using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yodo1.MAS;

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

    private void Start()
    {
        SetRewardDelegate();
    }

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
        if ((PlayerPrefs.GetInt("ActiveChallenge") > 1) && (GlobalVariables.gameNumber == 0 || (GlobalVariables.gameNumber == 2 && (PlayerPrefs.GetInt("ActiveChallenge") > 10)) || (GlobalVariables.gameNumber == 3 && (PlayerPrefs.GetInt("ActiveChallenge") <= 10))))
        {
            coinBox.gameObject.SetActive(false);
            coinButton.gameObject.SetActive(false);
            adButton.gameObject.SetActive(true);

            gameOverText.text = "Watch a short ad for an" + System.Environment.NewLine + "Extra Life";
        }
        else
        {
            coinBox.gameObject.SetActive(true);
            coinButton.gameObject.SetActive(true);
            adButton.gameObject.SetActive(false);

            coinText.text = PlayerPrefs.GetInt("TotalCoins").ToString();
            gameOverText.text = "Use 200 coins for an Extra Life";
        }
    }
    public void WatchForExtraLife()
    {
        if (Yodo1U3dMas.IsRewardedAdLoaded())
        {
            FindObjectOfType<AddsManager>().RemoveNetworkError();
            Yodo1U3dMas.ShowRewardedAd();
        }
        else
            FindObjectOfType<AddsManager>().ShowNetworkError();
    }
    void SetRewardDelegate()
    {
        Yodo1U3dMas.SetRewardedAdDelegate((Yodo1U3dAdEvent adEvent, Yodo1U3dAdError error) =>
        {
            Debug.Log("[Yodo1 Mas] RewardVideoDelegate:" + adEvent.ToString() + "\n" + error.ToString());
            switch (adEvent)
            {
                case Yodo1U3dAdEvent.AdClosed:
                    Debug.Log("[Yodo1 Mas] Reward video ad has been closed.");
                    break;
                case Yodo1U3dAdEvent.AdOpened:
                    Debug.Log("[Yodo1 Mas] Reward video ad has shown successful.");
                    break;
                case Yodo1U3dAdEvent.AdError:
                    Debug.Log("[Yodo1 Mas] Reward video ad error, " + error);
                    break;
                case Yodo1U3dAdEvent.AdReward:
                    Debug.Log("[Yodo1 Mas] Reward video ad reward, give rewards to the player for ExtraLife.");
                    HandleUserEarnedReward();
                    break;
            }

        });
    }
    void HandleUserEarnedReward()
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
