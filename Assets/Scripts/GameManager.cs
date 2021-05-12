using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    [SerializeField]GameObject gameMenu;
    [SerializeField]GameObject mainMenu;
    [SerializeField]GameObject gameOverMenu;
    [SerializeField]GameObject gamePauseMenu;
    [SerializeField]GameObject helpMenu;
    [SerializeField]GameObject rewardsMenu;
    [SerializeField]GameObject multiplierMenu;
    [SerializeField]GameObject objectGen;

    [SerializeField] Text gameMsgText;

    public void PlayGame()
    {
        mainMenu.SetActive(false);
        gameMenu.SetActive(true);
        objectGen.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void EndGame()
    {
        if (PlayerPrefs.GetInt("ActiveChallenge") > 1)
        {
            FindObjectOfType<AddsManager>().ShowInterstitial();
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void UseCoins()
    {
        int coins = PlayerPrefs.GetInt("TotalCoins");
        if (coins >= 200)
        {
            coins = coins - 200;
            PlayerPrefs.SetInt("TotalCoins", coins);
            GlobalVariables.gameNumber += 1;
            Time.timeScale = 0.01f;
            FindObjectOfType<PerksManager>().GameContinue();
            gameOverMenu.SetActive(false);
        }
        else
        {
            gameMsgText.text = "You do not have enough coins. Please End Game.";
        }
    }
    public void PauseGame()
    {
        gamePauseMenu.SetActive(true);
        Time.timeScale = 0;
        FindObjectOfType<CameraControl>().MoveCamera(false);
        EventManager.PGame();
    }
    public void UnpauseGame()
    {
        gamePauseMenu.SetActive(false);
        Time.timeScale = 1;
        FindObjectOfType<CameraControl>().MoveCamera(true);
        EventManager.RestartGame();
    }
    public void Help()
    {
        mainMenu.SetActive(false);
        helpMenu.SetActive(true);
    }
    public void GetRewardsMenu()
    {
        mainMenu.SetActive(false);
        rewardsMenu.SetActive(true);
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void NextMenu()
    {
        multiplierMenu.SetActive(false);
        gameOverMenu.SetActive(true);
    }
}
