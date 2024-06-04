using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemyMovement
{
    [SerializeField]
    private Transform leftPatrolPoint;
    [SerializeField]
    private Transform rightPatrolPoint;
    private bool isRight = true;
    private bool attacking = false;

    protected override void Start()
    {
        base.Start();

    }

    protected void Awake()
    {
        Initialize(2, 4, 2, false, 200, 1);
        player = GameObject.FindGameObjectWithTag("Player");

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void Update()
    {
        //Has Line of Sight
        if (canSeePlayer && !attacking)
        {
            engageState();
        }
        else if (!canSeePlayer && !attacking) 
        {
            patrolState();
        }

        Vector2 lookPlayer = player.transform.position - transform.position;
        //Direction of attack towards player
        if (lookPlayer.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //Check distance to player to determine if to attack
        if (Vector2.Distance(transform.position, player.transform.position) <= EnemyAttackRange)
        {
            if (!attacking)
            {
                StartCoroutine(stabAttack());
            }
        }
    }

    private IEnumerator stabAttack()
    {
        attacking = true;
        //Animation trigger for attack
        yield return new WaitForSeconds(0.5f); //length of enemy attack animation

        Collider2D hitPlayer = Physics2D.OverlapBox(transform.position + transform.right * transform.localScale.x * 0.5f,
            new Vector2(1, 1), 0, playerLayer);

        if (hitPlayer != null)
        {
            hitPlayer.GetComponent<PlayerHealth>().takeDamage(EnemyAttackDamage);
            Debug.Log("Hit Player");
        }

        yield return new WaitForSeconds(1f); //Enemy Attack Cooldown
        attacking = false;
    }

    protected void patrolState()
    {   
        if (isRight)
        {
            transform.Translate(Vector2.right * EnemyPatrolSpeed * Time.deltaTime);
        }
        else if (!isRight)
        {
            transform.Translate(Vector2.left * EnemyPatrolSpeed * Time.deltaTime);
        }
    }

    protected void engageState()
    {
        if (player.transform.position.x > transform.position.x)
        {
            transform.Translate(Vector2.right * EnemyChaseSpeed * Time.deltaTime);
        }
        if (player.transform.position.x < transform.position.x)
        {
            transform.Translate(Vector2.left * EnemyChaseSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PatrolPoint"))
        {
            isRight = !isRight;
        }

    }
}
