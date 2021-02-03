using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlowManager : MonoBehaviour
{
    [SerializeField] Slider modeSlider;

    void Start()
    {
        CheckMode();
    }
    void CheckMode()
    {
        modeSlider.value = PlayerPrefs.GetFloat("gameMode", 1);
        Camera.main.GetComponent<CameraControl>().SetCameraMode(modeSlider.value);
        FindObjectOfType<PerksManager>().GetComponent<MaterialChanger>().ChangeMaterialsForGameMode(modeSlider.value);
    }
    public void ChangeMode()
    {
        PlayerPrefs.SetFloat("gameMode", modeSlider.value);
        Camera.main.GetComponent<CameraControl>().SetCameraMode(modeSlider.value);
        FindObjectOfType<PerksManager>().GetComponent<MaterialChanger>().ChangeMaterialsForGameMode(modeSlider.value);
    }
}
