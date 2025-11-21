using UnityEngine;
using Unity.Behavior;
using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.UI;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class Enemy : MonoBehaviour
{
    GameObject mTarget;
    GameObject Target
    {
        get { return mTarget; }
        set
        {
            if (mTarget == value)
            {
                return;
            }
            if (value == null && mTarget != null)
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

    [SerializeField] float mLostTargetTime = 5f;

    [SerializeField] float mWalkSPD = 1;
    [SerializeField] float mChaseSPD = 3;

    [Header("Enemy Attack")]
    [SerializeField] Transform mAttackTransform;
    [SerializeField] float mAttackArea = 1;
    [SerializeField] float mAttackStrength = 20;
    [SerializeField] float mAttackInterval = 2f;


    [Header("PatrolPoints")]
    [SerializeField] List<GameObject> mPatrolPoints;

    [Header("Freeze")]
    [SerializeField] bool bIsFrozed;
    [SerializeField] private float mFrozeDuration;
    private float mFrozeTimer;

    BehaviorGraphAgent mBehaviorGraphAgent;
    NavMeshAgent mNavAgent;
    Animator mAnimator;
    private float loseTimer;

    private void Awake()
    {
        mBehaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        mNavAgent = GetComponent<NavMeshAgent>();
        mAnimator = GetComponent<Animator>();

        SetPatrolPoints();

        mBehaviorGraphAgent.BlackboardReference.SetVariableValue("Patrol Point", mPatrolPoints);
        mBehaviorGraphAgent.BlackboardReference.SetVariableValue("WalkSPD", mWalkSPD);
        mBehaviorGraphAgent.BlackboardReference.SetVariableValue("ChaseSPD", mChaseSPD);
    }

    private void SetPatrolPoints()
    {
        GameObject[] PT = GameObject.FindGameObjectsWithTag("PatrolPoints");
        foreach (GameObject patrolPoints in PT) 
        {
            mPatrolPoints.Add(patrolPoints);
        }
    }

    void Start()
    {
        
    }
    void Update()
    {
        PlayerSearch();
        FrozeTimer();
    }

    private void FrozeTimer()
    {
        if (!bIsFrozed) return;

        mFrozeTimer += Time.deltaTime;
        mNavAgent.isStopped = true;
        if (mFrozeTimer >= mFrozeDuration) 
        {
            UnFreeze();
            mFrozeTimer = 0;
        }
    }
    /*public void SetFrozen(bool frozen)
    {
        if (frozen)
        {
            // Stop movement by setting speeds to 0
            mNavAgent.isStopped = true;
        }
        else
        {
            // Restore original speeds
            mBehaviorGraphAgent.BlackboardReference.SetVariableValue("WalkSPD", mWalkSPD);
            mBehaviorGraphAgent.BlackboardReference.SetVariableValue("ChaseSPD", mChaseSPD);
        }

    }*/

    public void Freeze()
    {
        bIsFrozed = true;
        Debug.Log($"Enemy{this.name}, is frozed");
    }

    public void UnFreeze()
    {
        mNavAgent.isStopped = false;
        bIsFrozed = false;
        Debug.Log($"Enemy{this.name}, is unfrozed");
    }

    private void PlayerSearch()
    {

        Player player = GameManager.mGamaManager.mPlayer;

        //Debug.Log($"Player {player}");
        if (!player) { return; }


        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        bool isVisible = true;

        if (distanceToPlayer <= mAlwaysAwareDistance)
        {
            Target = player.gameObject;
            return;
        }

        if (distanceToPlayer > mSightDistance) 
        {
            isVisible = false;
            return;
        }

        Vector3 playerDir = (player.transform.position - transform.position).normalized;
        if (Vector3.Angle(playerDir, transform.forward) > mViewAngle)
        {
            isVisible = false;
            return;
        }
        Vector3 eyeViewPoint = transform.position + Vector3.up * mEyeHeight;
        if (Physics.Raycast(eyeViewPoint, playerDir, out RaycastHit hitInfo, mSightDistance))
        {
            if (hitInfo.collider.gameObject != player.gameObject)
            {
                isVisible = false;
                return;
            }
        }

        if (isVisible)
        {
            loseTimer = 0f;
            Target = player.gameObject;
        }
        else 
        {
            loseTimer += Time.deltaTime;
            if (loseTimer >= mLostTargetTime) 
            {
                Target = null;
            }
        }

        if (mNavAgent.remainingDistance <= mNavAgent.stoppingDistance) 
        {
            AttackPlayer();
        }
    }

    public void AttackPlayer() 
    {

        Collider[] collider = Physics.OverlapSphere(mAttackTransform.position, mAttackArea);

        foreach (Collider obj in collider) 
        {
            if (obj.CompareTag("Player")) 
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    
    }

    private IEnumerator AttackCoroutine() 
    {
        mAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(mAttackInterval);
    }

    public void Hit() 
    {
        Collider[] collider = Physics.OverlapSphere(mAttackTransform.position, mAttackArea);

        foreach (Collider obj in collider)
        {
            if (obj.CompareTag("Player"))
            {
                Player player = obj.GetComponent<Player>();
                player.TakeHealth(mAttackStrength);
            }
        }
    }




    private void OnDrawGizmos()
    {
        Vector3 eyeviewPoint = transform.position + Vector3.up * mEyeHeight;
        Gizmos.DrawWireSphere(eyeviewPoint, mSightDistance);
        Gizmos.DrawWireSphere(eyeviewPoint, mAlwaysAwareDistance);
        Gizmos.DrawWireSphere(mAttackTransform.position, mAttackArea);


        Vector3 leftLineDir = Quaternion.AngleAxis(mViewAngle, Vector3.up) * transform.forward;
        Vector3 rightLineDir = Quaternion.AngleAxis(-mViewAngle, Vector3.up) * transform.forward;
        Gizmos.DrawLine(eyeviewPoint, eyeviewPoint + leftLineDir * mSightDistance);
        Gizmos.DrawLine(eyeviewPoint, eyeviewPoint + rightLineDir * mSightDistance);

        if (Target)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, Target.transform.position);
            Gizmos.DrawWireSphere(Target.transform.position, 0.5f);
        }
    }




}
