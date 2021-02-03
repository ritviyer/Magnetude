using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using UnityEngine.PlayerLoop;
//LionStudios
using LionStudios;

public class AddsManager : MonoBehaviour
{
    string rewardedCoinAdUnitID;
    string rewardedMultiplierAdUnitID;
    string rewardedLifeAdUnitID;
    string interGameOverAdUnitID;
    string testDeviceID;

    [SerializeField] GameObject networkErrorMenu;

    private void Awake()
    {
        //LionStudios
        LionKit.Initialize();

        rewardedCoinAdUnitID = "ca-app-pub-2260161542736477/7687252901";
        rewardedMultiplierAdUnitID = "ca-app-pub-2260161542736477/6921305457";
        rewardedLifeAdUnitID = "ca-app-pub-2260161542736477/8725341719";
        interGameOverAdUnitID = "ca-app-pub-2260161542736477/9751337132";

        testDeviceID = "6B531390E44C6176899A493EC9AD7D0C";
    }
    private void Start()
    {
        //LionStudios
        //Commented all in start

        //MobileAds.Initialize(initStatus => {
        //    CheckNullAndCreate();
        //    InvokeRepeating("CheckAndLoadAds", 10f, 15f);
        //});
    }
    public void CheckNullAndCreate()
    {
        if (GlobalVariables.rewardedCoinAd == null)
            GlobalVariables.rewardedCoinAd = CreateAndLoadRewardedAd(rewardedCoinAdUnitID);
        if (GlobalVariables.rewardedMultiplierAd == null)
            GlobalVariables.rewardedMultiplierAd = CreateAndLoadRewardedAd(rewardedMultiplierAdUnitID);
        if (GlobalVariables.rewardedLifeAd == null)
            GlobalVariables.rewardedLifeAd = CreateAndLoadRewardedAd(rewardedLifeAdUnitID);
        if (GlobalVariables.interGameOverAd == null)
            GlobalVariables.interGameOverAd = CreateAndLoadInterstitialAd(interGameOverAdUnitID);
    }
    public void CheckAndLoadAds()
    {
        if (!GlobalVariables.rewardedCoinAd.IsLoaded())
            GlobalVariables.rewardedCoinAd = CreateAndLoadRewardedAd(rewardedCoinAdUnitID);
        if (!GlobalVariables.rewardedMultiplierAd.IsLoaded())
            GlobalVariables.rewardedMultiplierAd = CreateAndLoadRewardedAd(rewardedMultiplierAdUnitID);
        if (!GlobalVariables.rewardedLifeAd.IsLoaded())
            GlobalVariables.rewardedLifeAd = CreateAndLoadRewardedAd(rewardedLifeAdUnitID);
        if (!GlobalVariables.interGameOverAd.IsLoaded())
        {
            GlobalVariables.interGameOverAd.Destroy();
            GlobalVariables.interGameOverAd = CreateAndLoadInterstitialAd(interGameOverAdUnitID);
        }
    }
    RewardedAd CreateAndLoadRewardedAd(string adUnitId)
    {
        RewardedAd rewardedAd = new RewardedAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
        rewardedAd.OnAdClosed += HandleAdClosed;

        return rewardedAd;
    }
    InterstitialAd CreateAndLoadInterstitialAd(string adUnitId)
    {
        InterstitialAd interAd = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        interAd.LoadAd(request);
        interAd.OnAdClosed += HandleAdClosed;

        return interAd;
    }
 
    public void HandleAdClosed(object sender, EventArgs args)
    {
        CheckAndLoadAds();
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
