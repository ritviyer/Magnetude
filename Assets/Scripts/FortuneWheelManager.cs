using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class FortuneWheelManager : MonoBehaviour
{
    private bool _isStarted = false;
    private float[] _sectorsAngles;
    private float _finalAngle;
    private float _startAngle = 0;
    private float _currentLerpRotationTime;
    public Button TurnButton;
    public Button adTurnButton;
    public Button DailySpinButton;
    public Button BackButton;
    public GameObject Circle; 			// Rotatable Object with rewards
    public Text CoinsDeltaText; 		// Pop-up text with wasted or rewarded coins amount
    public Text CurrentCoinsText; 		// Pop-up text with wasted or rewarded coins amount
    int TurnCost = 150;			        // How much coins user waste when turn whe wheel
    int CurrentCoinsAmount;	            // Started coins amount. In your project it can be set up from CoinsManager or from PlayerPrefs and so on
    int PreviousCoinsAmount;		    // For wasted coins animation
    bool toPlayWheelSound = false;


    private void Start ()
    {
        CurrentCoinsAmount = PlayerPrefs.GetInt("TotalCoins");
        PreviousCoinsAmount = CurrentCoinsAmount;
        CurrentCoinsText.text = CurrentCoinsAmount.ToString ();
    }

    public void TurnWheel ()
    {
    	// Player has enough money to turn the wheel
        if (CurrentCoinsAmount >= TurnCost) {
            FindObjectOfType<SoundManager>().PlayFortuneWheelSound();
            _currentLerpRotationTime = 0f;
    	
    	    // Fill the necessary angles (for example if you want to have 12 sectors you need to fill the angles with 30 degrees step)
    	    _sectorsAngles = new float[] { 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330, 360 };
    	
    	    int fullCircles = 4;
    	    float randomFinalAngle = _sectorsAngles [UnityEngine.Random.Range (0, _sectorsAngles.Length)];

    	    // Here we set up how many circles our wheel should rotate before stop
    	    _finalAngle = -(fullCircles * 360 + randomFinalAngle);
    	    _isStarted = true;
            StartCoroutine(SpinTheWheel());

            PreviousCoinsAmount = CurrentCoinsAmount;
    	
    	    // Decrease money for the turn
    	    CurrentCoinsAmount -= TurnCost;

            // Animate coins
            HideCoinsDelta("-" + TurnCost);
            PlayerPrefs.SetString("MoneySpinAvailable", DateTime.Now.AddHours(1).ToString());
        }
    }

    public void TurnWheelForFree()
    {
        toPlayWheelSound = true;
        _currentLerpRotationTime = 0f;

        // Fill the necessary angles (for example if you want to have 12 sectors you need to fill the angles with 30 degrees step)
        _sectorsAngles = new float[] { 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330, 360 };

        int fullCircles = 4;
        float randomFinalAngle = _sectorsAngles[UnityEngine.Random.Range(0, _sectorsAngles.Length)];

        // Here we set up how many circles our wheel should rotate before stop
        _finalAngle = -(fullCircles * 360 + randomFinalAngle);
        _isStarted = true;
        StartCoroutine(SpinTheWheel());

        PreviousCoinsAmount = CurrentCoinsAmount;

        // Animate coins
        StartCoroutine(UpdateCoinsAmount());
    }

    private void GiveAwardByAngle ()
    {
    	// Here you can set up rewards for every sector of wheel
    	switch ((int)_startAngle) {
    	case 0:
    	    RewardCoins (300);
    	    break;
    	case -330:
    	    RewardCoins (100);
    	    break;
    	case -300:
    	    RewardCoins (250);
    	    break;
    	case -270:
    	    RewardCoins (200);
    	    break;
    	case -240:
    	    RewardCoins (150);
    	    break;
    	case -210:
    	    RewardCoins (100);
    	    break;
    	case -180:
    	    RewardCoins (0);
    	    break;
    	case -150:
    	    RewardCoins (200);
    	    break;
    	case -120:
    	    RewardCoins (150);
    	    break;
    	case -90:
    	    RewardCoins (100);
    	    break;
    	case -60:
    	    RewardCoins (50);
    	    break;
    	case -30:
    	    RewardCoins (200);
    	    break;
    	default:
    	    RewardCoins (150);
    	    break;
        }
    }
    void Update ()
    {
        if (toPlayWheelSound)
        {
            FindObjectOfType<SoundManager>().PlayFortuneWheelSound();
            toPlayWheelSound = false;
        }

        // Make turn button non interactable if user has not enough money for the turn
        if (_isStarted || CurrentCoinsAmount < TurnCost) {
    	    TurnButton.interactable = false;
    	    TurnButton.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
    	} else {
            TurnButton.interactable = true;
            TurnButton.GetComponent<Image>().color = new Color(255, 255, 255, 1);
    	}

        if (_isStarted)
        {
            adTurnButton.interactable = false;
            DailySpinButton.interactable = false;
            BackButton.interactable = false;
        }
        else
        {
            adTurnButton.interactable = true;
            DailySpinButton.interactable = true;
            BackButton.interactable = true;
        }
    }
    private IEnumerator SpinTheWheel()
    {
        float maxLerpRotationTime = 5f;

        while (_isStarted)
        {
            if(Time.unscaledDeltaTime<1f)
                _currentLerpRotationTime += Time.unscaledDeltaTime;
            else
                _currentLerpRotationTime += 0.02f;

            if (_currentLerpRotationTime > maxLerpRotationTime || Circle.transform.eulerAngles.z == _finalAngle)
            {
                _currentLerpRotationTime = maxLerpRotationTime;
                _isStarted = false;
                _startAngle = _finalAngle % 360;

                GiveAwardByAngle();
            }

            // Calculate current position using linear interpolation
            float t = _currentLerpRotationTime / maxLerpRotationTime;

            // This formulae allows to speed up at start and speed down at the end of rotation.
            // Try to change this values to customize the speed
            t = t * t * t * (t * (6f * t - 15f) + 10f);

            float angle = Mathf.Lerp(_startAngle, _finalAngle, t);
            Circle.transform.eulerAngles = new Vector3(0, 0, angle);

            yield return new WaitForEndOfFrame();
        }
    }

    void RewardCoins (int awardCoins)
    {
        CurrentCoinsAmount += awardCoins;
        HideCoinsDelta("+" + awardCoins);
    }

    void HideCoinsDelta(string s)
    {
        Text tempCoinsDelta = Instantiate(CoinsDeltaText, CoinsDeltaText.transform);
        tempCoinsDelta.text = s;
        LeanTween.move(tempCoinsDelta.gameObject, CurrentCoinsText.transform.position,1f).setDestroyOnComplete(true);
        StartCoroutine(UpdateCoinsAmount());
    }

    public void DailyRewardCoins(int awardCoins)
    {
        CurrentCoinsAmount += awardCoins;
        DailyHideCoinsDelta("+" + awardCoins);
    }

    void DailyHideCoinsDelta(string s)
    {
        Text tempCoinsDelta = Instantiate(CoinsDeltaText, DailySpinButton.transform.position, Quaternion.identity, CurrentCoinsText.transform);
        tempCoinsDelta.text = s;
        LeanTween.move(tempCoinsDelta.gameObject, CurrentCoinsText.transform.position, 1f).setDestroyOnComplete(true);
        StartCoroutine(UpdateCoinsAmount());
    }
    private IEnumerator UpdateCoinsAmount ()
    {
    	// Animation for increasing and decreasing of coins amount
    	const float seconds = 1.5f;
    	float elapsedTime = 0;
    
    	while (elapsedTime < seconds) {
    	    CurrentCoinsText.text = Mathf.Floor(Mathf.Lerp (PreviousCoinsAmount, CurrentCoinsAmount, (elapsedTime / seconds))).ToString ();
    	    elapsedTime += Time.deltaTime;
    
    	    yield return new WaitForEndOfFrame ();
        }
    
    	PreviousCoinsAmount = CurrentCoinsAmount;
    	CurrentCoinsText.text = CurrentCoinsAmount.ToString ();
        PlayerPrefs.SetInt("TotalCoins", CurrentCoinsAmount);
    }

    public void WatchForCoinSpin()
    {
        if (FindObjectOfType<AddsManager>().ShowVideoReward())
        {
            HandleUserEarnedReward();
        }
    }

    void HandleUserEarnedReward()
    {
        TurnWheelForFree();
        PlayerPrefs.SetString("AdSpinAvailable", DateTime.Now.AddHours(4).ToString());
    }
}
