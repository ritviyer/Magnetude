using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class MaterialChanger : MonoBehaviour
{
    [SerializeField] GameObject ball;
    [SerializeField] GameObject obstacle;
    [SerializeField] Material ballMaterialNormal;
    [SerializeField] Material obstacleMaterialNormal;
    [SerializeField] Material ballMaterialNeon;
    [SerializeField] Material obstacleMaterialNeon;
    [SerializeField] Material bombMaterial;
    Material ballMaterial;
    bool changeMaterial = false;

    private void Start()
    {
        ballMaterialNormal.color = new Color(ballMaterialNormal.color.r, ballMaterialNormal.color.b, ballMaterialNormal.color.g, 1f);
        ballMaterialNeon.color = new Color(ballMaterialNeon.color.r, ballMaterialNeon.color.b, ballMaterialNeon.color.g, 1f);
        bombMaterial.color = new Color(bombMaterial.color.r, bombMaterial.color.b, bombMaterial.color.g, 1f);
        ballMaterial = ball.GetComponent<MeshRenderer>().sharedMaterial;
    }
    private void OnEnable()
    {
        EventManager.onExitPerk += PerkActive;
        EventManager.onStickyBalls += ChangeBallMaterial;
        EventManager.onDiffuseBombs += ChangeBombMaterial;
    }
    private void OnDisable()
    {
        EventManager.onExitPerk -= PerkActive;
        EventManager.onStickyBalls -= ChangeBallMaterial;
        EventManager.onDiffuseBombs -= ChangeBombMaterial;
    }

    void ChangeBallMaterial()
    {
        changeMaterial = true;
        if (ball.GetComponent<TrailRenderer>().enabled)
            StartCoroutine(BlinkTrail());
        else
            StartCoroutine(ChangeAlpha(ballMaterial));
    }
    void ChangeBombMaterial()
    {
        changeMaterial = true;
        StartCoroutine(ChangeAlpha(bombMaterial));
    }
    void PerkActive()
    {
        changeMaterial = false;
        ballMaterialNormal.color = new Color(ballMaterialNormal.color.r, ballMaterialNormal.color.b, ballMaterialNormal.color.g, 1f);
        ballMaterialNeon.color = new Color(ballMaterialNeon.color.r, ballMaterialNeon.color.b, ballMaterialNeon.color.g, 1f);
        bombMaterial.color = new Color(bombMaterial.color.r, bombMaterial.color.b, bombMaterial.color.g, 1f);
        if (ball.GetComponent<TrailRenderer>().enabled)
        {
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Player");
            balls[0].GetComponent<TrailRenderer>().enabled = true;
            balls[1].GetComponent<TrailRenderer>().enabled = true;
        }
    }

    IEnumerator ChangeAlpha(Material material)
    {
        float r = material.color.r;
        float g = material.color.g;
        float b = material.color.b;
        float a = 1;
        Color color = new Color(r, g, b, a);
        float change = -0.05f;
        while (true)
        {
            if (!changeMaterial)
                break;
            a += change;
            color = new Color(r, g, b, a);
            material.color = color;
            if (a <= 0.2f)
                change = 0.05f;
            else if (a >= 0.95f)
                change = -0.05f;
            yield return new WaitForSeconds(0f);
        }
    }
    IEnumerator BlinkTrail()
    {
        bool blink = false;
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Player");
        while (true)
        {
            if (!changeMaterial)
                break;
            balls[0].GetComponent<TrailRenderer>().enabled = blink;
            balls[1].GetComponent<TrailRenderer>().enabled = blink;
            blink = !blink;
            yield return new WaitForSeconds(0.65f);
        }
    }
    public void ChangeMaterialsForGameMode(float modeValue)
    {
        if (modeValue == 1)
        {
            ball.GetComponent<MeshRenderer>().material = ballMaterialNeon;
            ball.GetComponent<TrailRenderer>().enabled = true;
            obstacle.GetComponent<MeshRenderer>().material = obstacleMaterialNeon;
        }
        else if(modeValue == 0)
        {
            ball.GetComponent<MeshRenderer>().material = ballMaterialNormal;
            ball.GetComponent<TrailRenderer>().enabled = false;
            obstacle.GetComponent<MeshRenderer>().material = obstacleMaterialNormal;
        }
        ballMaterial = ball.GetComponent<MeshRenderer>().sharedMaterial;
    }
}
