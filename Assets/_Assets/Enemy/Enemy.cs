using UnityEngine;
using Unity.Behavior;
using System;

public class Enemy : MonoBehaviour
{
    GameObject mTarget;
    GameObject Target
    {
        get { return mTarget; }
        set
        {
            if (Target == value)
            {
                return;
            }
            if (value == null)
            {
                mBehaviorGraphAgent.BlackboardReference.SetVariableValue("HasLastSeenPosition", true);
                mBehaviorGraphAgent.BlackboardReference.SetVariableValue("TargetLastSeenPosition", mTarget.transform.position);
            }
            mTarget = value;
            mBehaviorGraphAgent.BlackboardReference.SetVariableValue("Target", mTarget);
        }
    }

    [SerializeField] float mEyeHeight = 1.5f;
    [SerializeField] float mSightDistance = 5f;
    [SerializeField] float mViewAngle = 30f;
    [SerializeField] float mAlwaysAwareDistance = 1.5f;

    BehaviorGraphAgent mBehaviorGraphAgent;
    private void Awake()
    {
        mBehaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        PlayerSearch();
    }

    private void PlayerSearch()
    {

    }
}
