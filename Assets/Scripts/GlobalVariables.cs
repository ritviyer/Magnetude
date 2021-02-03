using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
public static class GlobalVariables
{
    public static int gameNumber;
    public static int gameCoins;
    public static int gameScore;
    public static int gameExtraLife;
    public static int gameSticky;
    public static int gameDiffusedBombs;
    public static int game2xScore;
    public static int game2xCoins;
    public static bool gameChallengeComplete;
    public static int canSpinMultiplier;
    public static RewardedAd rewardedCoinAd;
    public static RewardedAd rewardedMultiplierAd;
    public static RewardedAd rewardedLifeAd;
    public static InterstitialAd interGameOverAd;
}
