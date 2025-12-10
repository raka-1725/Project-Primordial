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

    private void Start()
    {
        GameManager.mGamaManager.onPlayerGoal += GameFinished;
        mPlayer.onPlayerDead += GameFinished;
        onGameStarted += GameStarted;

        if (!bClock) { mTime = mDuration; }

        mPlayer = FindAnyObjectByType<Player>();


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
        }

        if (mTime <= 0)
        {
            mTime = 0;

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
        }
    }

    
}
