using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeEnemy : BaseEnemyMovement
{
    [SerializeField]
    private Transform leftPatrolPoint;
    [SerializeField]
    private Transform rightPatrolPoint;
    private Vector2 moveDir = Vector2.left;
    private bool attacking = false;
    private Vector3 attackLocation;
    private bool playerPastBounds = false;

    //Anim
    public Animator enemyAnim;
    public SpriteRenderer enemyVisual;
    private EnemyHealthScript HPScript;

    //Player Location
    private Vector2 playerPos;
    private Vector2 enemyPos;
    private float yRange = 1.5f;
    private bool isWithinYRange = false;

    protected void Awake()
    {
        Initialize(2, 4, 2, false, 5, 1);
    }

    protected override void Start()
    {
        base.Start();
        HPScript = GetComponent<EnemyHealthScript>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void Update()
    {
        //Death Stop
        if (HPScript.enemyHealth <= 0)
        {
            EnemyPatrolSpeed = 0;
            EnemyChaseSpeed = 0;
        }

        //Range Calc
        playerPos = player.transform.position;
        enemyPos = transform.position;
        isWithinYRange = playerPos.y <= enemyPos.y + yRange && playerPos.y >= enemyPos.y - yRange;
        playerPastBounds = playerPos.x > rightPatrolPoint.position.x + 1f || 
            playerPos.x < leftPatrolPoint.position.x - 1f;

        //Has Line of Sight
        if (canSeePlayer && !attacking && !enemyBlind && !playerPastBounds && isWithinYRange)
        {
            engageState();
        }
        else if (!attacking)
        {
            patrolState();
        }



        //Patrol Direction
        if (transform.position.x >= rightPatrolPoint.position.x)
        {
            moveDir = Vector2.left;
        }
        if (transform.position.x <= leftPatrolPoint.position.x)
        {
            moveDir = Vector2.right;
        }


        Vector2 lookPlayer = player.transform.position - transform.position;
        //Direction of attack towards player
        if (lookPlayer.x > 0)
        {
            attackLocation = transform.position;
            attackLocation.x += 1;
        }
        else
        {
            attackLocation = transform.position;
            attackLocation.x -= 1;
        }

        //Check distance to player to determine if to attack
        if (Vector2.Distance(transform.position, player.transform.position) <= EnemyAttackRange)
        {
            if (!attacking)
            {
                enemyAnim.SetBool("EnemyChase", false);
                enemyAnim.SetBool("EnemyPatrol", false);
               
                
                StartCoroutine(stabAttack());
            }
        }

    }

    //Attacks
    private IEnumerator stabAttack()
    {
        attacking = true;
        //Animation trigger for attack
        enemyAnim.SetTrigger("Attack");
        //length of enemy attack animation

        yield return new WaitForSeconds(1f); //Enemy Attack Cooldown
        attacking = false;
    }

    //States
    protected void patrolState()
    {   
        transform.Translate(moveDir * EnemyPatrolSpeed * Time.deltaTime);

        //Animation
        enemyAnim.SetBool("EnemyPatrol", true);
        enemyAnim.SetBool("EnemyChase", false);

        if (moveDir == Vector2.right)
        {
            enemyVisual.flipX = true;
        }
        else
        {
            enemyVisual.flipX = false;
        }
    }

    protected void engageState()
    {
        if (player.transform.position.x > transform.position.x && 
            transform.position.x <= rightPatrolPoint.transform.position.x)
        {
            transform.Translate(Vector2.right * EnemyChaseSpeed * Time.deltaTime);
            enemyVisual.flipX = true;
        }
        if (player.transform.position.x < transform.position.x &&
            transform.position.x >= leftPatrolPoint.transform.position.x)
        {
            transform.Translate(Vector2.left * EnemyChaseSpeed * Time.deltaTime);
            enemyVisual.flipX = false;
        }
        //Animation
        enemyAnim.SetBool("EnemyPatrol", false);
        enemyAnim.SetBool("EnemyChase", true);
    }

    public void enemyAttackNumbers()
    {

        Collider2D hitPlayer = Physics2D.OverlapBox(attackLocation, new Vector2(1, 1), 0, playerLayer);

        if (hitPlayer != null)
        {
            hitPlayer.GetComponent<PlayerHealth>().takeDamage(EnemyAttackDamage);
            Debug.Log("Hit Player");
        }
    }

}
