using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraControl : MonoBehaviour
{
    [SerializeField]Renderer background;
    public bool moveCamera = false;
    public float cameraSpeed = 0f;
    Color bColor;
    Color gameOverColor;
    GameObject[] balls;

    void Awake()
    {
        Application.targetFrameRate = 120;

        float orthS = (6 / ((float)Screen.width / (float)Screen.height)) / 2;
        Camera.main.orthographicSize = orthS;
        float cameraY = (orthS * 2f) * 0.35f;
        transform.position = new Vector3(transform.position.x, cameraY, transform.position.z);
        Vector3 tempLimit = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        float widthLeft = tempLimit.x;
        float heightDown = tempLimit.y;

        tempLimit = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        float widthRight = tempLimit.x;
        float heightUp = tempLimit.y;

        background.transform.localScale = new Vector3(widthRight - widthLeft, heightUp - heightDown, 0f);
    }
    private void OnEnable()
    {
        EventManager.onStartGame += GameStarter;
        EventManager.onSpeedChange += IncreaseSpeed;
        EventManager.onRestartGame += ChangeBackground;
        EventManager.onPauseGame += ChangeBackground;
    }
    private void OnDisable()
    {
        EventManager.onStartGame -= GameStarter;
        EventManager.onSpeedChange -= IncreaseSpeed;
        EventManager.onRestartGame -= ChangeBackground;
        EventManager.onPauseGame -= ChangeBackground;
    }
    void IncreaseSpeed(float speed)
    {
        cameraSpeed = speed;
    }
    void GameStarter(float speed)
    {
        balls = GameObject.FindGameObjectsWithTag("Player");
        cameraSpeed = speed;
        MoveCamera(true);
    }
    private void FixedUpdate()
    {
        if (moveCamera)
        {
            balls[0].GetComponent<BallMovement>().MoveForward();
            balls[1].GetComponent<BallMovement>().MoveForward();
            if (Time.timeScale == 1)
                transform.position += Vector3.up * cameraSpeed;
        }
    }
    void Update()
    {
        if (moveCamera)
        {
            if(Time.timeScale!=1)
                background.material.SetColor("_Color", gameOverColor);
        }
    }
    public void MoveCamera(bool toMove)
    {
        moveCamera = toMove;
    }

    void ChangeBackground()
    {
        background.material.SetColor("_Color", bColor);
    }
    public void SetCameraMode(float modeValue)
    {
        if (modeValue == 1)
        {
            Camera.main.GetComponent<PostProcessLayer>().volumeLayer = LayerMask.GetMask("Glow");
            background.material.color = new Color(0.03529412f, 0.172549f, 0.2352941f, 1f);
            gameOverColor = new Color(0.35f, 0.06f, 0.06f);
        }
        else if (modeValue == 0)
        {
            Camera.main.GetComponent<PostProcessLayer>().volumeLayer = LayerMask.GetMask("Nothing");
            background.material.color = new Color(0.2039216f, 0.5529412f, 0.6313726f, 1f);
            gameOverColor = new Color(0.7f, 0.1f, 0.1f);
        }
        bColor = background.material.color;
    }
}
