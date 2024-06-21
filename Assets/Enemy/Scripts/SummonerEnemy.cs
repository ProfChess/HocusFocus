using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SummonerEnemy : BaseEnemyMovement
{
    //Bounds to stop direction movement
    public Transform leftLeash;
    public Transform rightLeash;

    //Running Variables
    private bool run = false;

    //Wondering Variables
    private int wonderFreq = 5;
    private Vector2 wonderDirection;
    public float wonderMinDistance = 2;
    public float wonderMaxDistance = 4;
    private Vector2 targetToWalk;
    private bool canWonder = false;
    private float idleTimer = 0f;

    protected void Awake()
    {
        Initialize(2, 4, 0, false, 100, 2);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void Start()
    {
        calculateTargetPosition();
    }
    protected void Update()
    {
        //Player gets too close -> summoner will run away
        run = player.transform.position.x <= transform.position.x + EnemyAttackRange &&
             player.transform.position.x >= transform.position.x - EnemyAttackRange;

        if (canWonder)
        {
            enemyWonderState();
        }
        else
        {
            idleTimer -= Time.deltaTime;
            if(idleTimer < 0f)
            {
                calculateTargetPosition();
                canWonder = true;
            }
        }
    }

    protected void enemyWonderState()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetToWalk, EnemyPatrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetToWalk) < 0.1f)
        {
            canWonder = false;
            idleTimer = wonderFreq;
        }
    }

    protected void calculateTargetPosition()
    {
        float wonderDir = Random.Range(1, 3);
        float wonderDistance = Random.Range(wonderMinDistance, wonderMaxDistance);
        if (wonderDir == 1)
        {
            wonderDirection = Vector2.right;
            targetToWalk = transform.position;
            targetToWalk.x += wonderDistance;
        }
        else if (wonderDir == 2)
        {
            wonderDirection = Vector2.left;
            targetToWalk = transform.position;
            targetToWalk.x -= wonderDistance;
        }
    }

    protected void enemyRunAwayState()
    {

    }

    protected void enemySummonState()
    {

    }

}
