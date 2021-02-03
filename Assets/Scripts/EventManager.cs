using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void StartGameDelegate(float speed);
    public static StartGameDelegate onStartGame;
    public static StartGameDelegate onSpeedChange;

    public delegate void ChoosePerkDelegate();
    public static ChoosePerkDelegate onStickyBalls;
    public static ChoosePerkDelegate onDiffuseBombs;
    public static ChoosePerkDelegate onDoubleCoins;
    public static ChoosePerkDelegate onDoublePoints;
    public static ChoosePerkDelegate onExitPerk;
    public static ChoosePerkDelegate onIncreasePerkTimer;

    public delegate void RestartGameDelegate();
    public static RestartGameDelegate onRestartGame;
    public static RestartGameDelegate onPauseGame;

    public static void StartGame(float speed)
    {
        if (onStartGame != null)
            onStartGame(speed);
    }
    public static void SpeedChange(float speed)
    {
        if (onSpeedChange != null)
            onSpeedChange(speed);
    }
    public static void StickyBalls()
    {
        if (onStickyBalls != null)
            onStickyBalls();
    }
    public static void DiffuseBombs()
    {
        if (onDiffuseBombs != null)
            onDiffuseBombs();
    }
    public static void DoubleCoins()
    {
        if (onDoubleCoins != null)
            onDoubleCoins();
    }
    public static void DoublePoints()
    {
        if (onDoublePoints != null)
            onDoublePoints();
    }
    public static void ExitPerk()
    {
        if (onExitPerk != null)
            onExitPerk();
    }
    public static void IncreasePerkTimer()
    {
        if (onIncreasePerkTimer != null)
            onIncreasePerkTimer();
    }
    public static void RestartGame()
    {
        if (onRestartGame != null)
            onRestartGame();
    }
    public static void PGame()
    {
        if (onPauseGame != null)
            onPauseGame();
    }
}
