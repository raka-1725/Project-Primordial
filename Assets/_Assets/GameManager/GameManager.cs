using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player mPlayerPrefab;
    public Player mPlayer => mPlayerPrefab;

    public static GameManager mGamaManager;

    public Action<GameManager> onPlayerGoal;

    private Clock mClock;

    private void Start()
    {
        mClock = GetComponent<Clock>();
    }

    private void OnDestroy()
    {
        if (mGamaManager == this) 
        {
            mGamaManager = null;
        }
    }

    private void Awake()
    {
        if (mGamaManager != null) 
        {
            Destroy(gameObject);
        }

        mGamaManager = this;
    }
}
