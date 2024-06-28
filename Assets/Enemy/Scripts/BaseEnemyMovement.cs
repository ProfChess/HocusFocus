using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class BaseEnemyMovement : MonoBehaviour
{

    public float EnemyPatrolSpeed;
    [HideInInspector]
    public float EnemyChaseSpeed;
    [HideInInspector]
    public float EnemyAttackDamage;
    [HideInInspector]
    public float EnemyDetectRange;
    [HideInInspector]
    public float EnemyAttackRange;
    public GameObject player;
    [HideInInspector]
    public bool canSeePlayer;
    [HideInInspector]
    public float tempSpeedNerf = 0f;
    [HideInInspector]
    private bool underStatus = false;
    [HideInInspector]
    public bool enemyBlind = false;

    public LayerMask playerLayer;
    private int visionLayerMask;
    public virtual void Initialize(float EnemyPatrolSpeed, float EnemyChaseSpeed, float EnemyAttackDamage, bool canSeePlayer, float EnemyDetectRange,
        float EnemyAttackRange)
    {
        this.EnemyPatrolSpeed = EnemyPatrolSpeed;
        this.EnemyChaseSpeed = EnemyChaseSpeed;
        this.EnemyAttackDamage = EnemyAttackDamage;
        this.canSeePlayer = canSeePlayer;
        this.EnemyDetectRange = EnemyDetectRange;
        this.EnemyAttackRange = EnemyAttackRange;
    }

    protected virtual void Start()
    {
        player = GameManager.Instance.returnPlayer();
    }

    protected virtual void FixedUpdate()
    {
        if (!enemyBlind)
        {
            visionLayerMask = LayerMask.GetMask("Ground", "Player");
        }
        else if (enemyBlind)
        {
            visionLayerMask = LayerMask.GetMask("Ground");
        }
        RaycastHit2D enemyVision = Physics2D.Raycast(transform.position, player.transform.position - transform.position, EnemyDetectRange, visionLayerMask); 
        if (enemyVision.collider != null)
        {
            canSeePlayer = enemyVision.collider.CompareTag("Player");
            if (canSeePlayer)
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
            }
        }
    }

    public virtual void slowDown(float amount, float duration)
    {
        tempSpeedNerf = amount;
        if (!underStatus)
        {
            EnemyPatrolSpeed -= tempSpeedNerf;
            EnemyChaseSpeed -= tempSpeedNerf;
            underStatus = true;
        }
        Invoke("restoreSpeed", duration);
    }

    public virtual void restoreSpeed()
    {
        EnemyPatrolSpeed += tempSpeedNerf;
        EnemyChaseSpeed += tempSpeedNerf;
        tempSpeedNerf = 0f;
        underStatus = false;
    }

    public virtual void blindness(float duration)
    {
        enemyBlind = true;
        Invoke("restoreSight", duration);
    }

    public virtual void restoreSight()
    {
        enemyBlind = false;
    } 


}
