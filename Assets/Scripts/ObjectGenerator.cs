using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] GameObject ball;
    [SerializeField] Obstacle obstacle;
    [SerializeField] GameObject coin;
    [SerializeField] GameObject bomb;
    [SerializeField] GameObject potion;
    [SerializeField] GameObject coinImage;

    float screenWidthLeft;
    float screenWidthRight;
    float screenWidth;
    float ballHeight = 0.6f;
    float ballWidth = 0.6f;
    float timer;
    float gameTimer;
    bool isNewHighScore = false;

    [SerializeField] Text coinText;
    [SerializeField] Text scoreText;
    [SerializeField] Text HighScoreText;


    float highScore;
    float yPosition = 0f;

    int obsGen = 2;
    bool isStarted = false;
    float perkTimer = -1f;
    int scoreInc = 1;
    int coinInc = 1;

    [SerializeField]GameObject pointer;
    GameObject pt;

    void Start()
    {
        timer = 0f;
        gameTimer = -1f;
        InitializeGlobalVariables();
        GetScreenDimensions();
        InstantiateBalls();
        InvokeRepeating("InstantiateObstacles",0f,25f);
        pt = Instantiate(pointer, new Vector3(0, 0, 0), Quaternion.identity);
        if (PlayerPrefs.HasKey("HighScoreP"))
        {
            highScore = PlayerPrefs.GetFloat("HighScoreP");
            HighScoreText.text = Mathf.Round(highScore).ToString();
        }
    }

    private void OnEnable()
    {
        EventManager.onStartGame += GameStarter;
        EventManager.onDoubleCoins += CoinDouble;
        EventManager.onDoublePoints += PointDouble;
        EventManager.onIncreasePerkTimer += PerkTimerIncrease;
        EventManager.onExitPerk += PerkExit;
    }
    private void OnDisable()
    {
        EventManager.onStartGame -= GameStarter;
        EventManager.onDoubleCoins -= CoinDouble;
        EventManager.onDoublePoints -= PointDouble;
        EventManager.onIncreasePerkTimer -= PerkTimerIncrease;
        EventManager.onExitPerk -= PerkExit;
    }
    void GameStarter(float speed)
    {
        FindObjectOfType<SoundManager>().PlayBackgroundMusic();
        Destroy(pt);
        InvokeRepeating("CalculateScore", 0f, 1f);
        InvokeRepeating("ChangeBallSpeed", 0.5f, 1f);

    }
    void CoinDouble()
    {
        perkTimer = 10f;
        coinInc = 2;
        LeanTween.scale(coinImage, new Vector3(0.7f, 0.7f, 0.7f), 0.5f).setLoopPingPong();
        StartCoroutine(PerkTimerStatus());
    }
    void PointDouble()
    {
        perkTimer = 10f;
        scoreInc = 2;
        LeanTween.scale(scoreText.gameObject, new Vector3(0.8f, 0.8f, 0.8f), 0.5f).setLoopPingPong();
        StartCoroutine(PerkTimerStatus());
    }
    void PerkTimerIncrease()
    {
        if(perkTimer!=-1f)
            perkTimer += 5f;
    }
    void PerkExit()
    {
        perkTimer = -1f;
        scoreInc = 1;
        coinInc = 1;
        coinImage.transform.localScale = new Vector3(1f, 1f, 1f);
        scoreText.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    IEnumerator PerkTimerStatus()
    {
        while (true)
        {
            perkTimer -= 1f;
            if (perkTimer == -1f)
                break;
            if (perkTimer >= 0 && perkTimer <= 2)
            {
                FindObjectOfType<SoundManager>().PlayTimerSound();
                FindObjectOfType<PerksManager>().AddText(perkTimer.ToString());
            }
            yield return new WaitForSeconds(1f);
        }
        EventManager.ExitPerk();
    }
    void Update()
    {
        if (!isStarted)
        {
            if (Input.GetMouseButton(0))
            {
                isStarted= true;
                EventManager.StartGame(0.05f);
            }
        }

        if(Time.frameCount%10 == 0)
        {
            if (GlobalVariables.gameScore > highScore)
            {
                if (!isNewHighScore)
                {
                    isNewHighScore = true;
                    FindObjectOfType<PerksManager>().AddPerkText("New High Score!!!");

                }
                highScore = GlobalVariables.gameScore;
                PlayerPrefs.SetFloat("HighScoreP", highScore);
                HighScoreText.text = Mathf.Round(highScore).ToString();
            }
        }
        else if (Time.frameCount % 10 == 5)
        {
            coinText.text = Mathf.Round(GlobalVariables.gameCoins).ToString();
            scoreText.text = Mathf.Round(GlobalVariables.gameScore).ToString();
        }
    }
    void ChangeBallSpeed()
    {
        if (gameTimer == 10)
            EventManager.SpeedChange(0.055f);
        else if (gameTimer == 20)
            EventManager.SpeedChange(0.06f);
        else if (gameTimer == 35)
            EventManager.SpeedChange(0.065f);
        else if (gameTimer == 50)
            EventManager.SpeedChange(0.07f);
        else if (gameTimer == 70)
            EventManager.SpeedChange(0.075f);
        else if (gameTimer == 90)
            EventManager.SpeedChange(0.08f);
        else if (gameTimer == 115)
            EventManager.SpeedChange(0.085f);
        else if (gameTimer == 140)
            EventManager.SpeedChange(0.09f);
    }
    void GetScreenDimensions()
    {
        Camera cam = FindObjectOfType<Camera>();
        float width = Screen.width;
        float height = Screen.height;

        Vector3 tempLimit = cam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        screenWidthLeft = tempLimit.x;

        tempLimit = cam.ScreenToWorldPoint(new Vector3(width, height, 0f));
        screenWidthRight = tempLimit.x;

        screenWidth = screenWidthRight - screenWidthLeft;
    }
    void InstantiateBalls()
    {
        float mid = (screenWidthLeft + screenWidthRight) / 2;
        float left = (screenWidthLeft + mid) / 2;
        float right = (screenWidthRight + mid) / 2;
        left = (screenWidthLeft + left) / 2;
        right = (screenWidthRight + right) / 2;

        Vector3 b1 = new Vector3(left, 0f, 0f);
        Vector3 b2 = new Vector3(right, 0f, 0f);
        Instantiate(ball, b1, Quaternion.identity);
        Instantiate(ball, b2, Quaternion.identity);
    }
    void CalculateScore()
    {
        GlobalVariables.gameScore += scoreInc;
        gameTimer += 1;

        DestroyObstacles();
    }
    void InstantiateObstacles()
    {
        for (int i = 0; i < 30; i++)
        {
            SelectObstacles();
        }
    }
    void SelectObstacles()
    {
        timer += 1;
        float yGap = Random.Range(ballHeight * 9f, ballHeight * 12f);
        yPosition += yGap;

        if (timer > 15f)
            obsGen = 7;
        else if (timer > 5f)
            obsGen = 5;
        
        int random = Random.Range(0, obsGen);

        if (random == 0)
            Center();
        else if (random == 1)
            Right();
        else if (random == 2)
            Left();
        else if (random == 3)
            LeftRight();
        else if (random == 4)
            MoveCenter();
        else if (random == 5)
            MoveSide();
        else if (random == 6)
            MixedObstacles();
    }

    void Center(float cent = 0f)
    {
        float cStart = screenWidthLeft + (ballWidth * 1.75f);
        float cEnd = cent - (ballWidth * 2.25f);
        GenerateObstacles(cStart, cEnd);
        if (Random.Range(0, 4) != 0)
        {
            if (Random.Range(0, 3) != 0)
                GenerateCoins(cEnd, yPosition);
            else
                GeneratePotion(cEnd, yPosition);
        }
        if (timer > 10f)
        {
            if (Random.Range(0, 2) == 0)
            {
                float yPosB;
                if (Random.Range(0, 2) == 0)
                    yPosB = yPosition;
                else
                    yPosB = yPosition + 2 * ballHeight;
                GenerateBomb(screenWidthLeft, cStart, yPosB);
            }
        }
    }
    void Left(float cent = 0f, bool bGen=true)
    {
        float cStart = screenWidthLeft;
        float cEnd = cent - (ballWidth * 2.25f);

        GenerateObstacles(cStart, cEnd);
        if (bGen)
        {
            if (timer > 10f)
            {
                if (Random.Range(0, 2) == 0)
                {
                    float yPosB;
                    yPosB = yPosition + (Random.Range(1, 5) * ballHeight);
                    float bStart = cEnd - bomb.transform.localScale.x;
                    GenerateBomb(bStart, cEnd, yPosB);
                }
            }
        }
    }
    void Right(float cent = 0f, bool bGen = true)
    {
        float cStart = screenWidthLeft + (ballWidth * 1.75f);
        float cEnd = cent;

        GenerateObstacles(cStart, cEnd);

        if (bGen)
        {
            if (timer > 5f)
            {
                if (Random.Range(0, 2) == 0)
                {
                    float yPosB;
                    yPosB = yPosition + (Random.Range(2, 4) * ballHeight);
                    GenerateBomb(screenWidthLeft, cStart, yPosB);
                }
            }
        }
    }
    void LeftRight(float cent = 0f, bool bGen = true)
    {
        float gapPos = (cent + screenWidthLeft) / 2;
        float cStart = screenWidthLeft;
        float cEnd = gapPos - (ballWidth * 1.25f);

        GenerateObstacles(cStart, cEnd);

        if (bGen)
        {
            if (timer > 15f)
            {
                if (Random.Range(0, 2) == 0)
                {
                    float yPosB;
                    yPosB = yPosition + (Random.Range(1, 4) * ballHeight);
                    float bStart = cEnd - bomb.transform.localScale.x;
                    GenerateBomb(bStart, cEnd, yPosB);
                }
            }
        }

        cStart = gapPos + (ballWidth * 1.25f);
        cEnd = cent;

        GenerateObstacles(cStart, cEnd);
       
        if (bGen)
        {
            if (Random.Range(0, 3) == 0)
            {
                GeneratePotion(cStart, yPosition - (potion.transform.localScale.y));
            }
            if (timer > 15f)
            {
                if (Random.Range(0, 2) == 0)
                {
                    float yPosB;
                    yPosB = yPosition + (Random.Range(1, 4) * ballHeight);
                    float bEnd = cStart + bomb.transform.localScale.x;
                    GenerateBomb(cStart, bEnd, yPosB);
                }
            }
        }
    }
    void MixedObstacles(float cent = 0f)
    {
        float iterations = Random.Range(3, 8);
        float gapIncrease = Random.Range(4, 6);
        float heightO = (ballWidth * iterations * gapIncrease) + ballHeight / 2;
        float posC = (yPosition + (heightO / 2)) - (ballHeight / 4);
        if (Random.Range(0, 2) == 0)
        {
            Obstacle temp = Instantiate(obstacle, new Vector3(cent, posC, 0f), Quaternion.identity);
            temp.transform.localScale = new Vector3(ballWidth / 2, heightO, 0f);
        }

        float center = cent - (ballWidth / 2);
        for (int i = 0; i <= iterations; i++)
        {
            int random = Random.Range(0, 3);
            if (random == 0)
                Left(center, false);
            if (random == 1)
                Right(center, false);
            if (random == 2)
                LeftRight(center, false);
            yPosition += ballWidth * gapIncrease;
        }
    }
    void MoveSide(float cent = 0f)
    {
        float cStart = screenWidthLeft;
        float cEnd = cent - (ballWidth);

        GenerateObstacles(cStart, cEnd, true);
        if (Random.Range(0, 5) != 0)
        {
            if (Random.Range(0, 3) != 0)
                GenerateCoins(cEnd - ballWidth, yPosition - ballHeight);
            else
                GeneratePotion(cEnd - potion.transform.localScale.x, yPosition - potion.transform.localScale.y);
        }
    }
    void MoveCenter()
    {
        float cStart = screenWidthLeft + (ballWidth * 1.5f);
        float cEnd = screenWidthRight - (ballWidth * 1.5f);

        GenerateObstacles(cStart, cEnd, true);
        if (Random.Range(0, 5) != 0)
        {
            if (Random.Range(0, 3) != 0)
                GenerateCoins(((cStart + cEnd) / 2) - (2 * ballWidth), yPosition - ballHeight);
            else
                GeneratePotion(((cStart + cEnd) / 2) - (2 * potion.transform.localScale.x),yPosition-potion.transform.localScale.y);
        }
    }
    void GenerateObstacles(float startPos, float endPos, bool toMove = false)
    {
        float length = endPos - startPos;
        if (toMove)
            length = length * 0.2f;
        float position = (startPos + endPos) / 2;
        float dir;
        if (Random.Range(0, 2) == 0)
            dir = 1f;
        else
            dir = -1f;

        Obstacle temp = Instantiate(obstacle, new Vector3(position, yPosition, 0f), Quaternion.identity);
        temp.transform.localScale = new Vector3(length, ballHeight / 2, 0f);
        if (toMove)
        {
            temp.MoveObstacle(startPos, endPos, length, dir);
        }
        if (position != -1 * position)
        {
            temp = Instantiate(obstacle, new Vector3(-1 * position, yPosition, 0f), Quaternion.identity);
            temp.transform.localScale = new Vector3(length, ballHeight / 2, 0f);
            if (toMove)
            {
                temp.MoveObstacle(-1 * endPos, -1 * startPos, length, dir * -1f);
            }
        }
    }
    void GenerateCoins(float boundary, float coinPos)
    {
        float coinW = coin.transform.localScale.x;
        float rem = ((-1f * boundary) - boundary) % coinW;
        rem = (rem / 2) + (coinW / 2);
        int count = 0;
        for (float i = boundary + rem; i < (-1f * boundary); i = i + coinW)
        {
            if (count == 4)
                break;
            Instantiate(coin, new Vector3(i, coinPos, 0f), Quaternion.identity);
            count += 1;
        }
    }

    void GenerateBomb(float startB, float endB, float bombPosY)
    {
        float bPos = (startB + endB) / 2;
        Instantiate(bomb, new Vector3(bPos, bombPosY, 0f), Quaternion.identity);
        Instantiate(bomb, new Vector3(bPos * -1f, bombPosY, 0f), Quaternion.identity);
    }

    void GeneratePotion(float boundary, float potionPosY)
    {
        float pPos = Random.Range(boundary + ((potion.transform.localScale.x*2)/3), 0- ((potion.transform.localScale.x*2)/3));
        Instantiate(potion, new Vector3(pPos, potionPosY, 0f), Quaternion.identity);
        Instantiate(potion, new Vector3(pPos * -1f, potionPosY, 0f), Quaternion.identity);
    }

    public void CoinPoints(GameObject coin)
    {
        FindObjectOfType<SoundManager>().PlayCoinPickupSound();
        GameObject cc = Instantiate(coinImage, Camera.main.WorldToScreenPoint(coin.transform.position), Quaternion.identity, coinImage.transform.parent.transform);
        Destroy(coin);
        LeanTween.move(cc, coinImage.transform.position, 1f).setEaseOutBounce().setDestroyOnComplete(true);
        GlobalVariables.gameCoins +=coinInc;
        int totCoin = 0;
        if (PlayerPrefs.HasKey("TotalCoins"))
            totCoin = PlayerPrefs.GetInt("TotalCoins");
        totCoin += coinInc;
        PlayerPrefs.SetInt("TotalCoins", totCoin);
    }

    void DestroyObstacles()
    {
        GameObject[] balls;
        balls = GameObject.FindGameObjectsWithTag("Player");
        float ballPosY = balls[0].transform.position.y < balls[1].transform.position.y ? balls[0].transform.position.y : balls[1].transform.position.y;
        float heightErase = ballPosY - (balls[0].transform.localScale.y * 5);

        string[] gameObjects = {"Bomb", "Coin", "Potion" };
        for (int ob = 0; ob < gameObjects.Length; ob++)
        {
            GameObject[] objects;
            objects = GameObject.FindGameObjectsWithTag(gameObjects[ob]);
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].transform.position.y <= heightErase)
                    Destroy(objects[i]);
            }
        }
    }
    void InitializeGlobalVariables()
    {
        GlobalVariables.gameNumber = 0;
        GlobalVariables.gameCoins = 0;
        GlobalVariables.gameScore = 0;
        GlobalVariables.gameExtraLife = 0;
        GlobalVariables.gameSticky = 0;
        GlobalVariables.gameDiffusedBombs = 0;
        GlobalVariables.game2xScore = 0;
        GlobalVariables.game2xCoins = 0;
        GlobalVariables.gameChallengeComplete = false;
        GlobalVariables.canSpinMultiplier = 0;
    }
}
