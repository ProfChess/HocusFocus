using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyMovement : MonoBehaviour
{
    [HideInInspector]
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

    public LayerMask playerLayer;

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

    }

    protected virtual void FixedUpdate()
    {
        int visionLayerMask = LayerMask.GetMask("Ground","Player");
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

}
