using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [HideInInspector]
    public string summonType;

    //Wondering Variables
    private int wonderFreq = 3;
    public float wonderMinDistance = 2;
    public float wonderMaxDistance = 4;
    private Vector2 targetToWalk;
    private bool canWonder = false;
    private float idleTimer = 0f;

    //Animation Variables
    public Animator enemyAnim;
    private SpriteRenderer sr;
    protected void Awake()
    {
        Initialize(2, 4, 0, false, 10, 4);

    }

    protected override void Start()
    {
        base.Start();
        sr = GetComponentInChildren<SpriteRenderer>();
        calculateTargetPosition();
    }
    protected void Update()
    {
        //Player gets too close -> summoner will run away
        if (player != null)
        {
            run = Vector3.Distance(transform.position, player.transform.position) <= EnemyAttackRange;
        }

        if (canWonder && !canSeePlayer || enemyBlind)
        {
            enemyWonderState();
        }
        else if (!canSeePlayer || enemyBlind) 
        {
            idleTimer -= Time.deltaTime;
            if(idleTimer < 0f)
            {
                calculateTargetPosition();
                canWonder = true;
            }
        }

        else if (canSeePlayer && !summoning && !run && canSummon && !enemyBlind)
        {
            summonType = "Single";
            enemySummonState();
        }

        else if ((canSeePlayer && !summoning && run && canRun && !enemyBlind) || runTriggered)
        {
            runTriggered = true;
            summonType = "Double";
            enemyRunAwayState();
        }
        else if (!canRun)
        {
            runCDTimer -= Time.deltaTime;
            canSummon = true;
            enemyAnim.SetBool("Running", false);
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
            enemyAnim.SetBool("Moving", false);
        }
        else
        {
            enemyAnim.SetBool("Moving", true);
        }
    }

    protected void calculateTargetPosition()
    {
        float wonderDir = Random.Range(1, 3);
        float wonderDistance = Random.Range(wonderMinDistance, wonderMaxDistance);
        if (wonderDir == 1) //Right
        {
            targetToWalk = transform.position;
            targetToWalk.x += wonderDistance;
            sr.flipX = false;
        }
        else if (wonderDir == 2) //Left
        {
            targetToWalk = transform.position;
            targetToWalk.x -= wonderDistance;
            sr.flipX = true;
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
                enemyAnim.SetBool("Running", false);
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
        enemyAnim.SetTrigger("Summoning");


        yield return new WaitForSeconds(1f); //Summoning cooldown
        summoning = false;
    }

    public void basicSummonAbility() //Called by animation event 
    {
        //Summoning Logic
        Vector3 summonPosition = transform.position;
        Instantiate(flyingEnemyPrefab, summonPosition, Quaternion.identity);
    }

    //Summons two flying enemies before running away
    private IEnumerator runSummon()
    {
        summoning = true;
        //animation
        enemyAnim.SetTrigger("Summoning");
        enemyAnim.SetBool("Running", true);
        yield return new WaitForSeconds(0.2f);

        summoning = false;

    }

    public void runSummonAbility() //Called by animation
    {
        //summoning
        Vector3 summonPositionA = transform.position;
        Vector3 summonPositionB = transform.position;
        summonPositionA.y += 0.5f;
        summonPositionB.y -= 0.5f;
        Instantiate(flyingEnemyPrefab, summonPositionA, Quaternion.identity);
        Instantiate(flyingEnemyPrefab, summonPositionB, Quaternion.identity);
    }

}
