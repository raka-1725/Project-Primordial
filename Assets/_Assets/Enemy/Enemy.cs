using UnityEngine;
using Unity.Behavior;
using System;
using UnityEditor.ShaderGraph.Internal;

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

        Player player = GameManager.mGamaManager.mPlayer;

        Debug.Log($"Player {player}");
        if (!player) { return; }


        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= mAlwaysAwareDistance)
        {
            Target = player.gameObject;
            return;
        }

        if (distanceToPlayer > mSightDistance) 
        {
            Target = player.gameObject;
            return;
        }

        if (distanceToPlayer > mSightDistance) 
        {
            Target = null;
            return;
        }

        Vector3 playerDir = (player.transform.position - transform.position).normalized;
        if (Vector3.Angle(playerDir, transform.forward) > mViewAngle)
        {
            Target = null;
            return;
        }
        Vector3 eyeViewPoint = transform.position + Vector3.up * mEyeHeight;
        if (Physics.Raycast(eyeViewPoint, playerDir, out RaycastHit hitInfo, mSightDistance))
        {
            if (hitInfo.collider.gameObject != player.gameObject)
            {
                Target = null;
                return;
            }
        }

        Target = player.gameObject;
    }


    private void OnDrawGizmos()
    {
        Vector3 eyeviewPoint = transform.position + Vector3.up * mEyeHeight;
        Gizmos.DrawWireSphere(eyeviewPoint, mSightDistance);
        Gizmos.DrawWireSphere(eyeviewPoint, mAlwaysAwareDistance);


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
