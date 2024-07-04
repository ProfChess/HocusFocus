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
    private Vector2 moveDir = Vector2.right;
    private bool attacking = false;

    protected void Awake()
    {
        Initialize(2, 4, 2, false, 5, 1);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void Update()
    {
        //Has Line of Sight
        if (canSeePlayer && !attacking && !enemyBlind)
        {
            engageState();
        }
        else if (!canSeePlayer && !attacking || enemyBlind) 
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
        transform.Translate(moveDir * EnemyPatrolSpeed * Time.deltaTime);
    }

    protected void engageState()
    {
        if (player.transform.position.x > transform.position.x && 
            transform.position.x <= rightPatrolPoint.transform.position.x)
        {
            transform.Translate(Vector2.right * EnemyChaseSpeed * Time.deltaTime);
        }
        if (player.transform.position.x < transform.position.x &&
            transform.position.x >= leftPatrolPoint.transform.position.x)
        {
            transform.Translate(Vector2.left * EnemyChaseSpeed * Time.deltaTime);
        }
    }



}
