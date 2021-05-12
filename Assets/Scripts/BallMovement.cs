using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.PlayerLoop;

public class BallMovement : MonoBehaviour
{
    public float sideSpeed = 0f;
    float reviveSideSpeed;
    //start with 0.05 to max 0.09
    public float forwardSpeed = 0f;
    public float reviveForwardSpeed;
    float direction;
    float screenLimitLeft;
    float screenLimitRight;

    float perkTimer = -1f;
    bool ballStick = false;
    bool bombDiffuse = false;
    bool isTouching = false;
    List<bool> isSizeIncreased = new List<bool>();
    static bool isGameOverCheck = false;

    //[SerializeField]GameObject pointer;
    //GameObject pt;
    private void Start()
    {
        isGameOverCheck = false;
        //pt = Instantiate(pointer, new Vector3(0, transform.position.y-1f, transform.position.z), Quaternion.identity);
        //pt.SetActive(false);
        if (transform.position.x < 0)
            direction = -1;
        else
            direction = 1;
        GetScreenLimit();
    }
    private void OnEnable()
    {
        EventManager.onStartGame += GameStarter;
        EventManager.onSpeedChange += IncreaseSpeed;
        EventManager.onStickyBalls += BallStick;
        EventManager.onDiffuseBombs += BombDiffuse;
        EventManager.onIncreasePerkTimer += PerkTimerIncrease;
        EventManager.onRestartGame += StartBalls;
        EventManager.onRestartGame += RevertToOrigDimensions;
        EventManager.onExitPerk += PerkExit;
        EventManager.onPauseGame += PauseBalls;
    }
    private void OnDisable()
    {
        EventManager.onStartGame -= GameStarter;
        EventManager.onSpeedChange -= IncreaseSpeed;
        EventManager.onStickyBalls -= BallStick;
        EventManager.onDiffuseBombs -= BombDiffuse;
        EventManager.onIncreasePerkTimer -= PerkTimerIncrease;
        EventManager.onRestartGame -= StartBalls;
        EventManager.onRestartGame -= RevertToOrigDimensions;
        EventManager.onExitPerk -= PerkExit;
        EventManager.onPauseGame -= PauseBalls;
    }
    void IncreaseSpeed(float speed)
    {
        if (Time.timeScale == 1)
        {
            forwardSpeed = speed;
        }
        reviveForwardSpeed = speed;
    }
    void GameStarter(float speed)
    {
        sideSpeed = 0.045f;
        forwardSpeed = speed;
        reviveSideSpeed = sideSpeed;
        reviveForwardSpeed = forwardSpeed;
    }
    void BallStick()
    {
        perkTimer = 10f;
        ballStick = true;
        StartCoroutine(PerkTimerStatus());
    }
    void BombDiffuse()
    {
        perkTimer = 10f;
        bombDiffuse = true;
        StartCoroutine(PerkTimerStatus(true));
    }
    void PerkTimerIncrease()
    {
        if (perkTimer != -1f)
            perkTimer += 5f;
    }
    void PerkExit()
    {
        perkTimer = -1f;
        bombDiffuse = false;
        ballStick = false;
    }
    IEnumerator PerkTimerStatus(bool isBomb = false)
    {
        while (true)
        {
            if(perkTimer!=-1f)
                perkTimer -= 1f;
            else if (perkTimer == -1f)
            {
                if (isBomb)
                    break;
                else
                {
                    if (!isTouching)
                        break;
                }
            }
            if (perkTimer >= 0 && perkTimer <= 2)
            {
                FindObjectOfType<SoundManager>().PlayTimerSound();
                FindObjectOfType<PerksManager>().AddText(perkTimer.ToString());
            }
            yield return new WaitForSeconds(1f);
        }
        EventManager.ExitPerk();
    }

    void IncreaseSize()
    {
        isSizeIncreased.Add(true);
        FindObjectOfType<SoundManager>().PlayBombPickupSound();
        float start = transform.localScale.x;
        transform.localScale = transform.localScale * 1.25f;
        float end = transform.localScale.x;
        float dimChange = (end - start) / 2;
        screenLimitLeft += dimChange;
        screenLimitRight -= dimChange;
        transform.position = (transform.position.x < screenLimitLeft) ? new Vector3(screenLimitLeft, transform.position.y, transform.position.z) : transform.position;
        transform.position = (transform.position.x > screenLimitRight) ? new Vector3(screenLimitRight, transform.position.y, transform.position.z) : transform.position;
        Invoke("RevertSize", 5f);
    }
    void RevertSize()
    {
        if (isSizeIncreased.Count <= 0)
            return;
        isSizeIncreased.RemoveAt(isSizeIncreased.Count - 1);
        float start = transform.localScale.x;
        transform.localScale = transform.localScale / 1.25f;
        float end = transform.localScale.x;
        float dimChange = (end - start) / 2;
        screenLimitLeft += dimChange;
        screenLimitRight -= dimChange;
    }
    public void MoveForward()
    {
        transform.position += Vector3.up * forwardSpeed;
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            if (transform.position.x > screenLimitLeft && transform.position.x < screenLimitRight)
                MoveOut();
        }
        else
        {
            MoveIn();
        }
    }

    void MoveOut()
    {
        float posL = Math.Abs(screenLimitLeft - transform.position.x);
        float posR = Math.Abs(screenLimitRight - transform.position.x);
        float pos = (posL < posR) ? posL : posR;
        if (pos < sideSpeed)
        {
            transform.Translate(Vector3.right * direction * pos);
        }
        else
        {
            transform.Translate(Vector3.right * direction * sideSpeed);
        }
        //pt.transform.position = new Vector3(0, transform.position.y - 1f, transform.position.z);
        //pt.SetActive(true);
    }
    void MoveIn()
    {
        if (ballStick && isTouching)
            return;
        transform.Translate(Vector3.right * -1*direction* sideSpeed);
        //pt.SetActive(false);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (ballStick)
            {
                isTouching = true;
                return;
            }
        }
        sideSpeed = 0;
        forwardSpeed = 0;
        if (!isGameOverCheck)
        {
            isGameOverCheck = true;
            FindObjectOfType<PerksManager>().CheckGameOver();
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isTouching = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")
        {
            FindObjectOfType<ObjectGenerator>().CoinPoints(other.gameObject);
        }
        else if (other.tag == "Potion")
        {
            Destroy(other.gameObject);
            FindObjectOfType<PerksManager>().PotionCollected();
        }
        else if (other.tag == "Bomb")
        {
            if (!bombDiffuse)
            {
                Invoke("IncreaseSize", 0.25f);
                Destroy(other.gameObject);
            }
        }
    }
    void GetScreenLimit()
    {
        Camera cam = FindObjectOfType<Camera>();
        float width = Screen.width;
        float height = Screen.height;

        Vector3 tempLimit = cam.ScreenToWorldPoint(new Vector3(0f,0f,0f));
        screenLimitLeft = tempLimit.x;
        
        tempLimit = cam.ScreenToWorldPoint(new Vector3(width, height, 0f));
        screenLimitRight = tempLimit.x;

        float dimension = (screenLimitRight - screenLimitLeft)*0.1f;
        screenLimitLeft += (dimension/2);
        screenLimitRight -= (dimension/2);
    }
    void RevertToOrigDimensions()
    {
        float numInc = isSizeIncreased.Count;
        for (int i = 0; i < numInc; i++)
            RevertSize();
    }
    public void StopBalls(float posY)
    {
        sideSpeed = 0;
        forwardSpeed = 0;
        Vector3 position = new Vector3((screenLimitRight - transform.localScale.x) * direction, posY, 0);
        transform.position = position;
    }
    void PauseBalls()
    {
        sideSpeed = 0;
        forwardSpeed = 0;
    }
    void StartBalls()
    {
        isGameOverCheck = false;
        sideSpeed = reviveSideSpeed;
        forwardSpeed = reviveForwardSpeed;
    }
}
