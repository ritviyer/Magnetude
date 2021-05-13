using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using Yodo1.MAS;

public class MultiplierWheelManager : MonoBehaviour
{
    private bool _isStarted = false;
    private float[] _sectorsAngles;
    private float _finalAngle;
    private float _startAngle = 0;
    private float _currentLerpRotationTime;
    public Button TurnButton;
    public Button adTurnButton;
    public Button forwardButton;
    public GameObject multiplierMenu;
    public GameObject Circle; 			// Rotatable Object with rewards
    public Text CurrentCoinsText; 		// Pop-up text with wasted or rewarded coins amount
    public Text CurrentScoreText; 		// Pop-up text with wasted or rewarded coins amount
    int CurrentCoinsAmount;	            // Started coins amount. In your project it can be set up from CoinsManager or from PlayerPrefs and so on
    int PreviousCoinsAmount;            // For wasted coins animation
    int CurrentScoreAmount;
    int PreviousScoreAmount;
    public Text CoinsDeltaText;
    public Text titleText;
    bool toPlayWheelSound = false;

    private void Start()
    {
        SetRewardDelegate();
    }
    public void TurnWheel()
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

        CurrentCoinsAmount = GlobalVariables.gameCoins;
        PreviousCoinsAmount = CurrentCoinsAmount;

        CurrentScoreAmount = GlobalVariables.gameScore;
        PreviousScoreAmount = CurrentScoreAmount;
    }

    private void GiveAwardByAngle()
    {
        GlobalVariables.canSpinMultiplier = 0;

        // Here you can set up rewards for every sector of wheel
        switch ((int)_startAngle)
        {
            case 0:
                StartCoroutine(ExtraLifeTimer());
                break;
            case -330:
                CoinMultiplier(1.1f);
                break;
            case -300:
                ScoreMultiplier(1.5f);
                break;
            case -270:
                CoinMultiplier(1.2f);
                break;
            case -240:
                ScoreMultiplier(1.4f);
                break;
            case -210:
                CoinMultiplier(1.3f);
                break;
            case -180:
                ScoreMultiplier(1.3f);
                break;
            case -150:
                CoinMultiplier(1.4f);
                break;
            case -120:
                ScoreMultiplier(1.2f);
                break;
            case -90:
                CoinMultiplier(1.5f);
                break;
            case -60:
                ScoreMultiplier(1.1f);
                break;
            case -30:
                CoinMultiplier(1.6f);
                break;
            default:
                ScoreMultiplier(1.1f);
                break;
        }
    }

    void Update()
    {
        if (toPlayWheelSound)
        {
            FindObjectOfType<SoundManager>().PlayFortuneWheelSound();
            toPlayWheelSound = false;
        }

        if (GlobalVariables.canSpinMultiplier == 1)
        {
            adTurnButton.gameObject.SetActive(false);
            TurnButton.gameObject.SetActive(true);
            TurnButton.interactable = true;
            forwardButton.interactable = false;
            titleText.text = "Challenge " + (PlayerPrefs.GetInt("ActiveChallenge") - 1).ToString() + " Complete!";
        }
        else if(GlobalVariables.canSpinMultiplier == 2)
        {
            adTurnButton.gameObject.SetActive(true);
            TurnButton.gameObject.SetActive(false);
            adTurnButton.interactable = true;
            forwardButton.interactable = true;
            titleText.text = "Try your Luck!";
        }
        else
        {
            TurnButton.interactable = false;
            adTurnButton.interactable = false;
            forwardButton.interactable = true;
        }

        if (_isStarted)
        {
            TurnButton.interactable = false;
            adTurnButton.interactable = false;
        }

        CurrentCoinsText.text = GlobalVariables.gameCoins.ToString();
        CurrentScoreText.text = GlobalVariables.gameScore.ToString();

        if (!GlobalVariables.gameChallengeComplete)
        {
            if (FindObjectOfType<PlayerProgressManager>().CheckChallengeCompletion())
                GlobalVariables.canSpinMultiplier = 1;
        }
    }
    private IEnumerator SpinTheWheel()
    {
        float maxLerpRotationTime = 5f;

        while (_isStarted)
        {
            if (Time.unscaledDeltaTime < 1f)
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
    private void FixedUpdate()
    {
        //if (!_isStarted)
        //    return;

        //float maxLerpRotationTime = 5f;

        //// increment timer once per frame
        //_currentLerpRotationTime += (1f / 60f);

        //if (_currentLerpRotationTime > maxLerpRotationTime || Circle.transform.eulerAngles.z == _finalAngle)
        //{
        //    _currentLerpRotationTime = maxLerpRotationTime;
        //    _isStarted = false;
        //    _startAngle = _finalAngle % 360;

        //    GiveAwardByAngle();
        //}

        //// Calculate current position using linear interpolation
        //float t = _currentLerpRotationTime / maxLerpRotationTime;

        //// This formulae allows to speed up at start and speed down at the end of rotation.
        //// Try to change this values to customize the speed
        //t = t * t * t * (t * (6f * t - 15f) + 10f);

        //float angle = Mathf.Lerp(_startAngle, _finalAngle, t);
        //Circle.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void HideCoinsDelta(string s)
    {
        Text tempCoinsDelta = Instantiate(CoinsDeltaText, CoinsDeltaText.transform);
        tempCoinsDelta.text = s;
        LeanTween.move(tempCoinsDelta.gameObject, CurrentCoinsText.transform.position, 1f).setIgnoreTimeScale(true).setDestroyOnComplete(true);
        StartCoroutine(UpdateCoinsAmount());
    }

    private IEnumerator UpdateCoinsAmount()
    {
        // Animation for increasing and decreasing of coins amount
        const float seconds = 1f;
        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            GlobalVariables.gameCoins = (int)Mathf.Floor(Mathf.Lerp(PreviousCoinsAmount, CurrentCoinsAmount, (elapsedTime / seconds)));
            elapsedTime += Time.unscaledDeltaTime;

            yield return new WaitForEndOfFrame();
        }
        GlobalVariables.gameCoins = CurrentCoinsAmount;
        int extraCoins = CurrentCoinsAmount - PreviousCoinsAmount;
        PreviousCoinsAmount = CurrentCoinsAmount;
        PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins")+extraCoins);
    }

    void HideScoreDelta(string s)
    {
        Text tempScoreDelta = Instantiate(CoinsDeltaText, CoinsDeltaText.transform);
        tempScoreDelta.text = s;
        LeanTween.move(tempScoreDelta.gameObject, CurrentScoreText.transform.position, 1f).setIgnoreTimeScale(true).setDestroyOnComplete(true);
        StartCoroutine(UpdateScoreAmount());
    }

    private IEnumerator UpdateScoreAmount()
    {
        // Animation for increasing and decreasing of coins amount
        const float seconds = 1f;
        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            GlobalVariables.gameScore = (int)Mathf.Floor(Mathf.Lerp(PreviousScoreAmount, CurrentScoreAmount, (elapsedTime / seconds)));
            elapsedTime += Time.unscaledDeltaTime;

            yield return new WaitForEndOfFrame();
        }
        GlobalVariables.gameScore = CurrentScoreAmount;
        PreviousScoreAmount = CurrentScoreAmount;
    }
    private IEnumerator ExtraLifeTimer()
    {
        // Animation for increasing and decreasing of coins amount
        const float seconds = 1f;
        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        ExtraLifeRestart();
    }

    void CoinMultiplier(float amount)
    {
        CurrentCoinsAmount = Mathf.RoundToInt(CurrentCoinsAmount * amount);
        HideCoinsDelta(amount.ToString() + 'x');
    }
    void ScoreMultiplier(float amount)
    {
        CurrentScoreAmount = Mathf.RoundToInt(CurrentScoreAmount * amount);
        HideScoreDelta(amount.ToString() + 'x');
    }
    void ExtraLifeRestart()
    {
        Time.timeScale = 0.01f;
        FindObjectOfType<PerksManager>().GameContinue();
        multiplierMenu.SetActive(false);
    }
    public void WatchForMultiplierSpin()
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
                    Debug.Log("[Yodo1 Mas] Reward video ad reward, give rewards to the player for multiplier spin.");
                    HandleUserEarnedReward();
                    break;
            }

        });
    }
    void HandleUserEarnedReward()
    {
        TurnWheel();
    }
}
