using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
//LionStudios
using LionStudios;

public class PerksManager : MonoBehaviour
{
    float potionCollected = 0f;
    bool perkActive = false;
    bool extraLife = false;

    [SerializeField] Text perkText;
    [SerializeField] Text TimerText;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject multiplierMenu;
    [SerializeField] GameObject cenTop;

    private void OnEnable()
    {
        EventManager.onExitPerk += PerkActive;
    }
    private void OnDisable()
    {
        EventManager.onExitPerk -= PerkActive;
    }
    private void Update()
    {
        if (potionCollected == 2f)
            ChoosePerk();
    }

    public void PotionCollected()
    {
        potionCollected++;
    }

    void PerkActive()
    {
        perkActive = false;
    }

    void ChoosePerk()
    {
        FindObjectOfType<SoundManager>().PlayPerkPickupSound();
        potionCollected = 0;
        if (perkActive)
        {
            EventManager.IncreasePerkTimer();
            AddPerkText("+5 Seconds");
        }
        else
        {
            int rand;
            if (extraLife || GlobalVariables.gameExtraLife>0)
                rand = Random.Range(1, 9);
            else
                rand = Random.Range(1, 10);

            if (rand <= 2)
            {
                GlobalVariables.game2xCoins += 1;
                EventManager.DoubleCoins();
                perkActive = true;
                AddPerkText("2X Coins");
            }
            else if (rand > 2 && rand <= 4)
            {
                GlobalVariables.gameDiffusedBombs += 1;
                EventManager.DiffuseBombs();
                perkActive = true;
                AddPerkText("Diffused Bombs");
            }
            else if (rand > 4 && rand <= 6)
            {
                GlobalVariables.game2xScore += 1;
                EventManager.DoublePoints();
                perkActive = true;
                AddPerkText("2X Score");
            }
            else if (rand > 6 && rand <= 8)
            {
                GlobalVariables.gameSticky += 1;
                EventManager.StickyBalls();
                perkActive = true;
                AddPerkText("Sticky");
            }
            else if (rand == 9)
            {
                GlobalVariables.gameExtraLife += 1;
                extraLife = true;
                AddPerkText("Extra Life");
            }
        }
    }
    public void AddText(string s)
    {
        TimerText.text = s;
        Invoke("EraseText", 1f);
    }
    public void AddPerkText(string s)
    {
        Text tt = Instantiate(perkText, perkText.transform);
        tt.text = s;
        LeanTween.scale(tt.gameObject, new Vector3(0.7f, 0.7f, 0.7f), 0.4f).setLoopPingPong(1);
        LeanTween.move(tt.gameObject, cenTop.transform.position, 1f).setEaseLinear().delay = 0.8f;
        LeanTween.scale(tt.gameObject, new Vector3(0.6f, 0.6f, 0.6f), 0.9f).setDestroyOnComplete(true).delay = 0.8f;
    }
    void EraseText()
    {
        TimerText.text = "";
    }

    public void CheckGameOver()
    {
        FindObjectOfType<SoundManager>().StopBackgroundMusic();
        LeanTween.cancelAll();
        EventManager.ExitPerk();
        EraseText();
        FindObjectOfType<SoundManager>().PlayDeathSound();
        if (!extraLife)
        {
            if (!GlobalVariables.gameChallengeComplete)
            {
                if (FindObjectOfType<PlayerProgressManager>().CheckChallengeCompletion())
                {
                    GlobalVariables.canSpinMultiplier = 1;
                    multiplierMenu.SetActive(true);

                    //LionStudios
                    Dictionary<string, object> eventParams = new Dictionary<string, object>();
                    eventParams["level"] = PlayerPrefs.GetInt("ActiveChallenge");
                    eventParams["game Coins"] = GlobalVariables.gameCoins;
                    eventParams["game score"] = GlobalVariables.gameScore;
                    Analytics.Events.LevelComplete(eventParams);
                }
                //LionStudios
                //all comments in this function added
                else
                {
                    //if ((GlobalVariables.gameNumber == 2 || GlobalVariables.gameNumber == 3 || GlobalVariables.gameNumber == 5) && (PlayerPrefs.GetInt("ActiveChallenge") > 3))
                    //{
                    //    if(GlobalVariables.gameNumber == 5)
                    //    {
                    //        GlobalVariables.canSpinMultiplier = 2;
                    //        multiplierMenu.SetActive(true);
                    //    }
                    //    else if (GlobalVariables.gameNumber == 3 && (PlayerPrefs.GetInt("ActiveChallenge") > 10))
                    //    {
                    //        GlobalVariables.canSpinMultiplier = 2;
                    //        multiplierMenu.SetActive(true);
                    //    }
                    //    else if (GlobalVariables.gameNumber == 2 && (PlayerPrefs.GetInt("ActiveChallenge") <= 10))
                    //    {
                    //        if(Random.Range(0,2) == 0)
                    //        {
                    //            GlobalVariables.canSpinMultiplier = 2;
                    //            multiplierMenu.SetActive(true);
                    //        }
                    //        else
                    //            gameOverMenu.SetActive(true);
                    //    }
                    //    else
                    //        gameOverMenu.SetActive(true);
                    //}
                    //else
                    //    gameOverMenu.SetActive(true);

                    //remove this line later
                    gameOverMenu.SetActive(true);

                    //LionStudios
                    Dictionary<string, object> eventParams = new Dictionary<string, object>();
                    eventParams["level"] = PlayerPrefs.GetInt("ActiveChallenge");
                    eventParams["game Coins"] = GlobalVariables.gameCoins;
                    eventParams["game score"] = GlobalVariables.gameScore;
                    Analytics.Events.LevelFailed(eventParams);
                }
            }
            else
            {
                //native banner add
                gameOverMenu.SetActive(true);

                //LionStudios
                Dictionary<string, object> eventParams = new Dictionary<string, object>();
                eventParams["level"] = PlayerPrefs.GetInt("ActiveChallenge");
                eventParams["game Coins"] = GlobalVariables.gameCoins;
                eventParams["game score"] = GlobalVariables.gameScore;
                Analytics.Events.LevelFailed(eventParams);
            }
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 0.01f;
            Invoke("GameContinue", 0.02f);
        }
    }
    public void GameContinue()
    {
        GameObject[] balls;
        balls = GameObject.FindGameObjectsWithTag("Player");
        float ballPosY = balls[0].transform.position.y < balls[1].transform.position.y ? balls[0].transform.position.y : balls[1].transform.position.y;
        float heightErase = ballPosY + (balls[0].transform.localScale.y * 7);

        string[] gameObjects = { "Obstacle", "Bomb", "Coin", "Potion" };
        for (int ob = 0; ob < gameObjects.Length; ob++)
        {
            GameObject[] objects;
            objects = GameObject.FindGameObjectsWithTag(gameObjects[ob]);
            for (int i =0; i<objects.Length; i++)
            {
                if (objects[i].transform.position.y <= heightErase)
                    Destroy(objects[i]);
            }
        }

        for (int ball = 0; ball<balls.Length; ball++)
        {
            balls[ball].GetComponent<BallMovement>().StopBalls(ballPosY);
        }
        StartCoroutine(RestartGameTimer(3));
    }

    IEnumerator RestartGameTimer(int number)
    {
        while (number>0)
        {
            FindObjectOfType<SoundManager>().PlayTimerSound();
            TimerText.text = "Extra life..." + System.Environment.NewLine + number.ToString();
            number -= 1;
            yield return new WaitForSeconds(0.01f);
        }
        EraseText();
        Time.timeScale = 1;
        EventManager.RestartGame();
        extraLife = false;
    }
}
