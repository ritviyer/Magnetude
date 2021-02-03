using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerProgressManager : MonoBehaviour
{
    DateTime t = new DateTime(2020, 10, 29, 23, 10, 20);
    void Awake()
    {
        if (!PlayerPrefs.HasKey("DayNum"))
        {
            PlayerPrefs.SetInt("DayNum", 1);
            PlayerPrefs.SetString("DayDateLast", DateTime.Today.ToString("MM/dd/yyyy"));
            PlayerPrefs.SetString("DayDateNext", DateTime.Today.AddDays(1).ToString("MM/dd/yyyy"));
            GetDailyBonusText();
            PlayerPrefs.SetInt("ActiveChallenge", 1);
            PlayerPrefs.SetString("MoneySpinAvailable", DateTime.Now.ToString());
            PlayerPrefs.SetString("AdSpinAvailable", DateTime.Now.ToString());
            PlayerPrefs.SetInt("DailyBonusCollected", 0);
            PlayerPrefs.SetInt("TotalCoins", 0);
            PlayerPrefs.SetInt("PlayMusic", 1);
        }
    }
    private void Start()
    {
        CheckStreak();
        GetChallengesText();
    }

    void CheckStreak()
    {
        string dateToday = DateTime.Today.ToString("MM/dd/yyyy");

        if (dateToday == PlayerPrefs.GetString("DayDateLast"))
            return;
        else if (dateToday == PlayerPrefs.GetString("DayDateNext"))
        {
            PlayerPrefs.SetInt("DayNum", PlayerPrefs.GetInt("DayNum") + 1);
            PlayerPrefs.SetString("DayDateLast", DateTime.Today.ToString("MM/dd/yyyy"));
            PlayerPrefs.SetString("DayDateNext", DateTime.Today.AddDays(1).ToString("MM/dd/yyyy"));
        }
        else
        {
            PlayerPrefs.SetInt("DayNum", 1);
            PlayerPrefs.SetString("DayDateLast", DateTime.Today.ToString("MM/dd/yyyy"));
            PlayerPrefs.SetString("DayDateNext", DateTime.Today.AddDays(1).ToString("MM/dd/yyyy"));
        }
        GetDailyBonusText();
    }
    void GetDailyBonusText()
    {
        int day = PlayerPrefs.GetInt("DayNum");
        int coins = day * 25;
        if (coins > 500)
            coins = 500;
        string bonusText= " ";
        if (day <= 6)
        {
            bonusText = "Day " + day.ToString() + Environment.NewLine + Environment.NewLine + "You win " + coins.ToString() + Environment.NewLine + "bonus coins!";
        }
        else if (day >= 7)
        {
            bonusText = "Day " + day.ToString() + Environment.NewLine + Environment.NewLine + "You win " + coins.ToString() + " bonus coins and a free spin!";
        }
        PlayerPrefs.SetString("DailyBonusText", bonusText);
        PlayerPrefs.SetInt("DailyBonusCollected", 0);
        PlayerPrefs.SetInt("DailyBonusAmount", coins);
    }
    void GetChallengesText()
    {
        int activeChallenge = PlayerPrefs.GetInt("ActiveChallenge");
        string challengeText = " ";
        switch (activeChallenge)
        {
            case 1:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Beat the score of 20";
                break;
            case 2:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Collect 10 coins in a game";
                break;
            case 3:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Collect the extra life perk atleast once";
                break;
            case 4:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Beat the score of 50";
                break;
            case 5:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Collect 2 different perks in a game";
                break;
            case 6:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Collect 35 coins in a game";
                break;
            case 7:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Collect 3 different perks in a game";
                break;
            case 8:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Beat the score of 100";
                break;
            case 9:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Collect 50 coins in a game";
                break;
            case 10:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Collect 3 same perks in a game";
                break;
            case 11:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Beat the score of 150";
                break;
            case 12:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Collect 75 coins in a game";
                break;
            case 13:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Beat the score of 250";
                break;
            case 14:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Collect all 5 different perks in a game";
                break;
            case 15:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Collect 100 coins in a game";
                break;
            case 16:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Beat the score of 500";
                break;
            case 17:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Collect 5 same perks in a game";
                break;
            case 18:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Beat the score of 1000";
                break;
            case 19:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Collect 200 coins in a game";
                break;
            case 20:
                challengeText = "Level " + activeChallenge.ToString() + Environment.NewLine + Environment.NewLine + "Beat your highscore!";
                break;
            default:
                challengeText = "You have completed all the challenges." + Environment.NewLine + Environment.NewLine + "More to follow soon.";
                break;
        }
        PlayerPrefs.SetString("ChallengeText", challengeText);
    }

    public bool CheckChallengeCompletion()
    {
        int activeChallenge = PlayerPrefs.GetInt("ActiveChallenge");
        bool challengeComplete = false;

        switch (activeChallenge)
        {
            case 1:
                if (GlobalVariables.gameScore >= 20)
                    challengeComplete = true;
                break;
            case 2:
                if (GlobalVariables.gameCoins >= 10)
                    challengeComplete = true;
                break;
            case 3:
                if (GlobalVariables.gameExtraLife > 0)
                    challengeComplete = true;
                break;
            case 4:
                if (GlobalVariables.gameScore >= 50)
                    challengeComplete = true;
                break;
            case 5:
                int totalPerks5 = 0;
                if (GlobalVariables.gameExtraLife > 0)
                    totalPerks5 += 1;
                if (GlobalVariables.gameSticky > 0)
                    totalPerks5 += 1;
                if (GlobalVariables.gameDiffusedBombs > 0)
                    totalPerks5 += 1;
                if (GlobalVariables.game2xCoins > 0)
                    totalPerks5 += 1;
                if (GlobalVariables.game2xScore > 0)
                    totalPerks5 += 1;

                if (totalPerks5>=2)
                    challengeComplete = true;
                break;
            case 6:
                if (GlobalVariables.gameCoins >= 35)
                    challengeComplete = true;
                break;
            case 7:
                int totalPerks7 = 0;
                if (GlobalVariables.gameExtraLife > 0)
                    totalPerks7 += 1;
                if (GlobalVariables.gameSticky > 0)
                    totalPerks7 += 1;
                if (GlobalVariables.gameDiffusedBombs > 0)
                    totalPerks7 += 1;
                if (GlobalVariables.game2xCoins > 0)
                    totalPerks7 += 1;
                if (GlobalVariables.game2xScore > 0)
                    totalPerks7 += 1;

                if (totalPerks7 >= 3)
                    challengeComplete = true;
                break;
            case 8:
                if (GlobalVariables.gameScore >= 100)
                    challengeComplete = true;
                break;
            case 9:
                if (GlobalVariables.gameCoins >= 50)
                    challengeComplete = true;
                break;
            case 10:
                int totalPerks10 = 0;
                if (GlobalVariables.gameExtraLife >=3)
                    totalPerks10 += 1;
                if (GlobalVariables.gameSticky >=3)
                    totalPerks10 += 1;
                if (GlobalVariables.gameDiffusedBombs >= 3)
                    totalPerks10 += 1;
                if (GlobalVariables.game2xCoins >= 3)
                    totalPerks10 += 1;
                if (GlobalVariables.game2xScore >= 3)
                    totalPerks10 += 1;

                if (totalPerks10 > 0)
                    challengeComplete = true;
                break;
            case 11:
                if (GlobalVariables.gameScore >= 150)
                    challengeComplete = true;
                break;
            case 12:
                if (GlobalVariables.gameCoins >= 75)
                    challengeComplete = true;
                break;
            case 13:
                if (GlobalVariables.gameScore >= 250)
                    challengeComplete = true;
                break;
            case 14:
                int totalPerks14 = 0;
                if (GlobalVariables.gameExtraLife > 0)
                    totalPerks14 += 1;
                if (GlobalVariables.gameSticky > 0)
                    totalPerks14 += 1;
                if (GlobalVariables.gameDiffusedBombs > 0)
                    totalPerks14 += 1;
                if (GlobalVariables.game2xCoins > 0)
                    totalPerks14 += 1;
                if (GlobalVariables.game2xScore > 0)
                    totalPerks14 += 1;

                if (totalPerks14 == 5)
                    challengeComplete = true;
                break;
            case 15:
                if (GlobalVariables.gameCoins >= 100)
                    challengeComplete = true;
                break;
            case 16:
                if (GlobalVariables.gameScore >= 500)
                    challengeComplete = true;
                break;
            case 17:
                int totalPerks17 = 0;
                if (GlobalVariables.gameExtraLife >= 5)
                    totalPerks17 += 1;
                if (GlobalVariables.gameSticky >= 5)
                    totalPerks17 += 1;
                if (GlobalVariables.gameDiffusedBombs >= 5)
                    totalPerks17 += 1;
                if (GlobalVariables.game2xCoins >= 5)
                    totalPerks17 += 1;
                if (GlobalVariables.game2xScore >= 5)
                    totalPerks17 += 1;

                if (totalPerks17 > 0)
                    challengeComplete = true;
                break;
            case 18:
                if (GlobalVariables.gameScore >= 1000)
                    challengeComplete = true;
                break;
            case 19:
                if (GlobalVariables.gameCoins >= 200)
                    challengeComplete = true;
                break;
            case 20:
                if (GlobalVariables.gameScore >= PlayerPrefs.GetFloat("HighScoreP"))
                    challengeComplete = true;
                break;
            default:
                break;
        }
        if (challengeComplete)
        {
            PlayerPrefs.SetInt("ActiveChallenge", activeChallenge + 1);
            GlobalVariables.gameChallengeComplete = true;
        }
        return challengeComplete;
    }
}
