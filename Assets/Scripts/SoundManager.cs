using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField]AudioSource backgroundSound;
    [SerializeField]AudioSource buttonSound;
    [SerializeField]AudioSource coinPickupSound;
    [SerializeField]AudioSource perkPickupSound;
    [SerializeField]AudioSource bombPickupSound;
    [SerializeField]AudioSource timerSound;
    [SerializeField]AudioSource deathSound;
    [SerializeField]AudioSource fortuneWheelSound;

    [SerializeField]Sprite musicOn;
    [SerializeField]Sprite musicOff;
    [SerializeField]Button musicButton;
    private void Awake()
    {
        ChangeMusicIcon();
    }
    private void OnEnable()
    {
        EventManager.onRestartGame += PlayBackgroundMusic;
    }
    private void OnDisable()
    {
        EventManager.onRestartGame -= PlayBackgroundMusic;
    }
    public void PlayBackgroundMusic()
    {
        backgroundSound.Play();
        backgroundSound.volume = 1f;
    }
    public void StopBackgroundMusic()
    {
        backgroundSound.Pause();
    }
    public void PlayButtonSound()
    {
        buttonSound.Play();
    }
    public void PlayCoinPickupSound()
    {
        DecreaseBackgroundVolume();
        coinPickupSound.Play();
        Invoke("IncreaseBackgroundVolume", 1f);
    }
    public void PlayPerkPickupSound()
    {
        DecreaseBackgroundVolume();
        perkPickupSound.Play();
        Invoke("IncreaseBackgroundVolume", 1f);
    }
    public void PlayBombPickupSound()
    {
        DecreaseBackgroundVolume();
        bombPickupSound.Play();
        Invoke("IncreaseBackgroundVolume", 1.5f);
    }
    public void PlayTimerSound()
    {
        DecreaseBackgroundVolume();
        timerSound.Play();
        Invoke("IncreaseBackgroundVolume", 1f);
    }
    public void PlayDeathSound()
    {
        DecreaseBackgroundVolume();
        deathSound.Play();
        Invoke("IncreaseBackgroundVolume", 1f);
    }
    void IncreaseBackgroundVolume()
    {
        backgroundSound.volume = 1f;
    }
    void DecreaseBackgroundVolume()
    {
        backgroundSound.volume = 0.05f;
    }
    public void PlayFortuneWheelSound()
    {
        fortuneWheelSound.Play();
    }
    public void ToPlayMusic()
    {
        if (PlayerPrefs.GetInt("PlayMusic") == 1)
            PlayerPrefs.SetInt("PlayMusic", 0);
        
        else if (PlayerPrefs.GetInt("PlayMusic") == 0)
            PlayerPrefs.SetInt("PlayMusic", 1);

        ChangeMusicIcon();
    }
    void ChangeMusicIcon()
    {
        if (PlayerPrefs.GetInt("PlayMusic") == 1)
        {
            musicButton.image.sprite = musicOn;
            backgroundSound.mute = false;
            buttonSound.mute = false;
            coinPickupSound.mute = false;
            perkPickupSound.mute = false;
            bombPickupSound.mute = false;
            timerSound.mute = false;
            deathSound.mute = false;
            fortuneWheelSound.mute = false;
        }
        else if (PlayerPrefs.GetInt("PlayMusic") == 0)
        {
            musicButton.image.sprite = musicOff;
            backgroundSound.mute = true;
            buttonSound.mute = true;
            coinPickupSound.mute = true;
            perkPickupSound.mute = true;
            bombPickupSound.mute = true;
            timerSound.mute = true;
            deathSound.mute = true;
            fortuneWheelSound.mute = true;
        }
    }
}
