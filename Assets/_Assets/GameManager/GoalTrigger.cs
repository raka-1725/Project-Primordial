using System;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    GameManager mGameManager;
    private void Awake()
    {
        mGameManager = FindAnyObjectByType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            mGameManager.onPlayerGoal.Invoke(mGameManager);
        }
    }
}
