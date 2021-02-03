using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class GameOverCalculator : MonoBehaviour
{
    [SerializeField] Text coinText;
    [SerializeField] GameObject GoRestartMenu;
    [SerializeField] GameObject GoMenuwithoutRestart;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] Button coinButton;
    [SerializeField] Button adButton;
    [SerializeField] Button coinBox;
    [SerializeField] Text gameOverText;

    bool adSeen = false;
    void Update()
    {
        if (adSeen)
        {
            adSeen = false;
            GiveRestartReward();
        }

        if (GlobalVariables.gameNumber >= 5)
        {
            GoRestartMenu.SetActive(false);
            GoMenuwithoutRestart.SetActive(true);
        }
        else
        {
            GoRestartMenu.SetActive(true);
            GoMenuwithoutRestart.SetActive(false);
            CheckCoinOrAd();
        }

    }
    void CheckCoinOrAd()
    {
        //LionStudios
        //all comments added in this function
        //if ((PlayerPrefs.GetInt("ActiveChallenge") > 1) && (GlobalVariables.gameNumber == 0 || (GlobalVariables.gameNumber == 2 && (PlayerPrefs.GetInt("ActiveChallenge") > 10)) || (GlobalVariables.gameNumber == 3 && (PlayerPrefs.GetInt("ActiveChallenge") <= 10))))
        //{
        //    coinBox.gameObject.SetActive(false);
        //    coinButton.gameObject.SetActive(false);
        //    adButton.gameObject.SetActive(true);

        //    gameOverText.text = "Watch a short ad for an" + System.Environment.NewLine + "Extra Life";
        //}
        //else
        //{
        //    coinBox.gameObject.SetActive(true);
        //    coinButton.gameObject.SetActive(true);
        //    adButton.gameObject.SetActive(false);

        //    coinText.text = PlayerPrefs.GetInt("TotalCoins").ToString();
        //    gameOverText.text = "Use 200 coins for an Extra Life";
        //}

        //remove these lines
        coinText.text = PlayerPrefs.GetInt("TotalCoins").ToString();
    }
    public void WatchForExtraLife()
    {
        if (GlobalVariables.rewardedLifeAd == null)
            FindObjectOfType<AddsManager>().CheckNullAndCreate();
        else if (GlobalVariables.rewardedLifeAd.IsLoaded())
        {
            GlobalVariables.rewardedLifeAd.OnUserEarnedReward += HandleUserEarnedReward;
            FindObjectOfType<AddsManager>().RemoveNetworkError();
            GlobalVariables.rewardedLifeAd.Show();
        }
        else
        {
            FindObjectOfType<AddsManager>().ShowNetworkError();
            FindObjectOfType<AddsManager>().CheckAndLoadAds();
        }
    }

    void HandleUserEarnedReward(object sender, Reward args)
    {
        adSeen = true;
    }
    void GiveRestartReward()
    {
        GlobalVariables.gameNumber += 1;
        gameOverMenu.SetActive(false);
        Time.timeScale = 0.01f;
        FindObjectOfType<PerksManager>().GameContinue();
    }
}
