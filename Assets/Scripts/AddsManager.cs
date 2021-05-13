using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.PlayerLoop;
using Yodo1.MAS;


public class AddsManager : MonoBehaviour
{
    [SerializeField] GameObject networkErrorMenu;

    private void Start()
    {
        Yodo1U3dMas.InitializeSdk();
    }
    public void ShowInterstitial()
    {
        if(Yodo1U3dMas.IsInterstitialAdLoaded())
            Yodo1U3dMas.ShowInterstitialAd();
    }

    public bool ShowVideoReward()
    {
        if (Yodo1U3dMas.IsRewardedAdLoaded())
        {
            RemoveNetworkError();
            Yodo1U3dMas.ShowRewardedAd();
            return RewardedVideoEvents();
        }
        else
            ShowNetworkError();

        return false;
    }

    bool RewardedVideoEvents()
    {
        bool ret = false;
        Yodo1U3dMas.SetRewardedAdDelegate((Yodo1U3dAdEvent adEvent, Yodo1U3dAdError error) => {
            switch (adEvent)
            {
                case Yodo1U3dAdEvent.AdClosed:
                    break;
                case Yodo1U3dAdEvent.AdOpened:
                    break;
                case Yodo1U3dAdEvent.AdError:
                    ShowNetworkError();
                    break;
                case Yodo1U3dAdEvent.AdReward:
                    ret = true;
                    break;
            }
        });
        return ret;
    }

    void ShowNetworkError()
    {
        networkErrorMenu.SetActive(true);
    }
    void RemoveNetworkError()
    {
        networkErrorMenu.SetActive(false);
    }

}
