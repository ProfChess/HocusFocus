using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.XR;
using UnityEngine;

public class SummonerEnemy : BaseEnemyMovement
{
    //Bounds to stop direction movement
    public Transform leftLeash;
    public Transform rightLeash;

    //Running Variables
    private bool run = false;
    private int runningFreq = 4;
    private float runCDTimer = 0f;
    private bool canRun = false;
    private Vector2 dashTarget;
    private bool runTriggered = false;

    //Summoning
    public GameObject flyingEnemyPrefab;
    private bool summoning = false;
    private bool canSummon = true;
    bool hasSummoned = false;

    //Wondering Variables
    private int wonderFreq = 3;
    public float wonderMinDistance = 2;
    public float wonderMaxDistance = 4;
    private Vector2 targetToWalk;
    private bool canWonder = false;
    private float idleTimer = 0f;

    protected void Awake()
    {
        Initialize(2, 4, 0, false, 10, 4);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void Start()
    {
        calculateTargetPosition();
    }
    protected void Update()
    {
        //Player gets too close -> summoner will run away
        run = Vector3.Distance(transform.position, player.transform.position) <= EnemyAttackRange;

        if (canWonder && !canSeePlayer)
        {
            enemyWonderState();
        }
        else if (!canSeePlayer) 
        {
            idleTimer -= Time.deltaTime;
            if(idleTimer < 0f)
            {
                calculateTargetPosition();
                canWonder = true;
            }
        }

        else if (canSeePlayer && !summoning && !run && canSummon)
        {
            enemySummonState();
        }

        else if ((canSeePlayer && !summoning && run && canRun) || runTriggered)
        {
            runTriggered = true;
            enemyRunAwayState();
        }
        else if (!canRun)
        {
            runCDTimer -= Time.deltaTime;
            canSummon = true;
            if(runCDTimer < 0f)
            {
                findDashTarget();
                canRun = true;
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
            targetToWalk = transform.position;
            targetToWalk.x += wonderDistance;
        }
        else if (wonderDir == 2)
        {
            targetToWalk = transform.position;
            targetToWalk.x -= wonderDistance;
        }

        //Does not let summmoner walk out of bounds
        if (targetToWalk.x >= rightLeash.position.x)
        {
            targetToWalk.x = rightLeash.position.x;
        }
        if (targetToWalk.x <= leftLeash.position.x)
        {
            targetToWalk.x = leftLeash.position.x;
        }
    }

    protected void findDashTarget()
    {
        //dash target
        float distanceToRight = Vector2.Distance(transform.position, rightLeash.position);
        float distanceToLeft = Vector2.Distance(transform.position, leftLeash.position);
        
        if (distanceToRight >= distanceToLeft)
        {
            dashTarget = rightLeash.position;
        }
        else if (distanceToLeft >= distanceToRight)
        {
            dashTarget = leftLeash.position;
        }

    }
    protected void enemyRunAwayState()
    {   
        if (!hasSummoned)
        {
            StartCoroutine(runSummon());
            hasSummoned = true;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, EnemyChaseSpeed * Time.deltaTime);
            canSummon = false;
            if (Vector2.Distance(transform.position, leftLeash.position) <= 0.1f ||
                Vector2.Distance(transform.position, rightLeash.position) <= 0.1f)
            {
                canRun = false;
                runCDTimer = runningFreq;
                hasSummoned = false;
                runTriggered = false;
                findDashTarget();
            }
        }

    }

    protected void enemySummonState()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= EnemyDetectRange &&
            Vector2.Distance(transform.position, player.transform.position) > EnemyAttackRange)
        {
            StartCoroutine(summonFlyingEnemy());
        }
    }

    //Summons a flying enemy
    private IEnumerator summonFlyingEnemy()
    {
        summoning = true;
        //Animation trigger

        yield return new WaitForSeconds(0.5f); //Length of summoning anim

        //Summoning Logic
        Vector3 summonPosition = transform.position;
        summonPosition.x += 1;
        summonPosition.y += 1;
        Instantiate(flyingEnemyPrefab, summonPosition, Quaternion.identity);

        yield return new WaitForSeconds(1f); //Summoning cooldown
        summoning = false;
    }

    //Summons two flying enemies before running away
    private IEnumerator runSummon()
    {
        summoning = true;
        //animation

        yield return new WaitForSeconds(0.2f);

        //summoning
        Vector3 summonPositionA = transform.position;
        Vector3 summonPositionB = transform.position;
        summonPositionA.x += 1;
        summonPositionA.y += 2;

        summonPositionB.x -= 1;
        summonPositionB.y += 2;
        Instantiate(flyingEnemyPrefab, summonPositionA, Quaternion.identity);
        Instantiate(flyingEnemyPrefab, summonPositionB, Quaternion.identity);

        summoning = false;

    }

}
