using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;
using System.Data.Common;

public class RewardsManager : MonoBehaviour
{
    [SerializeField] Text dbText;
    [SerializeField] Text chText;
    [SerializeField] Button dailySpin;
    [SerializeField] Button dailySpinTimer;
    [SerializeField] Button spinButton;
    [SerializeField] Button adSpinButton;
    [SerializeField] Button spinTimerButton;
    void Update()
    {
        dbText.text = PlayerPrefs.GetString("DailyBonusText");
        chText.text = PlayerPrefs.GetString("ChallengeText");
        if (Time.frameCount % 10 == 0)
            CheckDailySpin();
        else if (Time.frameCount % 10 == 5)
            CheckSpin();
    }
    public void CollectDailyReward()
    {
        int reward = PlayerPrefs.GetInt("DailyBonusAmount");
        int day = PlayerPrefs.GetInt("DayNum");
        GetComponentInParent<FortuneWheelManager>().DailyRewardCoins(reward);
        if (day >= 7)
            GetComponentInParent<FortuneWheelManager>().TurnWheelForFree();
        string bonusText = "Unlock your next bonus in...";
        PlayerPrefs.SetString("DailyBonusText", bonusText);
        PlayerPrefs.SetInt("DailyBonusCollected", 1);
    }
    void CheckDailySpin()
    {
        if (PlayerPrefs.GetInt("DailyBonusCollected") == 1)
        {
            dailySpin.gameObject.SetActive(false);
            dailySpinTimer.gameObject.SetActive(true);
            dailySpinTimer.GetComponentInChildren<Text>().text = TimeToBonus(DateTime.Now, DateTime.Today.AddDays(1));
        }
        else
        {
            dailySpin.gameObject.SetActive(true);
            dailySpinTimer.gameObject.SetActive(false);
        }
    }
    void CheckSpin()
    {
        DateTime coinAvail = Convert.ToDateTime(PlayerPrefs.GetString("MoneySpinAvailable"));
        DateTime adAvail = Convert.ToDateTime(PlayerPrefs.GetString("AdSpinAvailable"));

        //LionStudios
        //all comments in this function are added for this

        //if (DateTime.Now > adAvail)
        //{
        //    adSpinButton.gameObject.SetActive(true);
        //    spinButton.gameObject.SetActive(false);
        //    spinTimerButton.gameObject.SetActive(false);
        //}
        //Make this elseif
        if (DateTime.Now > coinAvail)
        {
            spinButton.gameObject.SetActive(true);
            adSpinButton.gameObject.SetActive(false);
            spinTimerButton.gameObject.SetActive(false);
        }
        else
        {
            spinButton.gameObject.SetActive(false);
            adSpinButton.gameObject.SetActive(false);
            spinTimerButton.gameObject.SetActive(true);

            //remove this timer and uncomment if else
            spinTimerButton.GetComponentInChildren<Text>().text = TimeToBonus(DateTime.Now, coinAvail);

            //if(adAvail < coinAvail)
            //    spinTimerButton.GetComponentInChildren<Text>().text = TimeToBonus(DateTime.Now, adAvail);
            //else
            //    spinTimerButton.GetComponentInChildren<Text>().text = TimeToBonus(DateTime.Now, coinAvail);
        }
    }
    string TimeToBonus(DateTime from, DateTime to)
    {
        int hours = (int)(to - from).TotalHours;
        int min = (int)(to - from).Minutes;
        int sec = (int)(to - from).Seconds;
        string s = hours.ToString() + ":" + min.ToString() + ":" + sec.ToString();

        return s;
    }
}
