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
        SetDelegates();
        Yodo1U3dMas.InitializeSdk();
    }
    void SetDelegates()
    {
        Yodo1U3dMas.SetInitializeDelegate((bool success, Yodo1U3dAdError error) =>
        {
            Debug.Log("[Yodo1 Mas] InitializeDelegate, success:" + success + ", error: \n" + error.ToString());
        });

        Yodo1U3dMas.SetInterstitialAdDelegate((Yodo1U3dAdEvent adEvent, Yodo1U3dAdError error) =>
        {
            Debug.Log("[Yodo1 Mas] InterstitialAdDelegate:" + adEvent.ToString() + "\n" + error.ToString());
            switch (adEvent)
            {
                case Yodo1U3dAdEvent.AdClosed:
                    Debug.Log("[Yodo1 Mas] Interstital ad has been closed.");
                    break;
                case Yodo1U3dAdEvent.AdOpened:
                    Debug.Log("[Yodo1 Mas] Interstital ad has been shown.");
                    break;
                case Yodo1U3dAdEvent.AdError:
                    Debug.Log("[Yodo1 Mas] Interstital ad error, " + error.ToString());
                    break;
            }

        });
    }
    public void ShowInterstitial()
    {
        if(Yodo1U3dMas.IsInterstitialAdLoaded())
            Yodo1U3dMas.ShowInterstitialAd();
    }

    public void ShowNetworkError()
    {
        networkErrorMenu.SetActive(true);
    }
    public void RemoveNetworkError()
    {
        networkErrorMenu.SetActive(false);
    }

}
