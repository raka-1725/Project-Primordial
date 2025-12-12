using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private float mTime;
    [SerializeField] private float mDurationToPlayerDeath;
    private float mPlayerDeathTime;
    [Header("Settings")]
    public static bool bClock;
    [SerializeField] private bool bStop;
    [SerializeField] private float mDuration;
    private float damageTimer = 0f;


    public Action<Clock> onGameFinished;
    public Action<Clock> onGameStarted;

    Player mPlayer;
    ClockUI mClocUI;

    private void Start()
    {
        mPlayer = FindAnyObjectByType<Player>();
        mClocUI = FindAnyObjectByType<ClockUI>();
        GameManager.mGamaManager.onPlayerGoal += GameFinished;
        mPlayer.onPlayerDead += GameFinished;
        onGameStarted += GameStarted;

        if (!bClock) { mTime = mDuration; }


        bClock = GameMode.Instance.bClock;
    }

    private void GameFinished(Player player)
    {
        bStop = true;
    }

    private void GameFinished(GameManager manager)
    {
        bStop = true;
    }

    private void GameStarted(Clock clock)
    {
        bStop = false;
        Debug.Log("ClockStart");
    }


    private void Update()
    {
        TimeCount();
        TimeCountDown();
    }

    private void TimeCountDown()
    {
        if (!bClock && !bStop && mTime > 0)
        {
            mTime -= Time.deltaTime;
            mClocUI.ShowTime(mTime);
        }

        if (mTime <= 0)
        {
            mTime = 0;
            mClocUI.ShowTime(0);
            mPlayerDeathTime += Time.deltaTime;
            damageTimer += Time.deltaTime;

            if (damageTimer >= 1f)
            {
                damageTimer -= 1f;
                mPlayer.TakeHealth(100f / mDurationToPlayerDeath);
            }
        }
    }

    private void TimeCount()
    {
        if (!bStop && bClock) 
        {
            mTime += Time.deltaTime;
            mClocUI.ShowTime(mTime);
        }
    }

    
}
