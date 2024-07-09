using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : BaseEnemyMovement
{
    protected void Awake()
    {
        Initialize(0, 2, 3, true, 1, 1);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player)
        {
            Vector2 chaseDirection = player.transform.position - transform.position;
            gameObject.GetComponentInChildren<SpriteRenderer>().flipX = player.transform.position.x < transform.position.x;
            if (enemyBlind)
            {
                chaseDirection = -chaseDirection;
                transform.Translate(chaseDirection.normalized * EnemyChaseSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(chaseDirection.normalized * EnemyChaseSpeed * Time.deltaTime);
            }

        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<EnemyHealthScript>().dyingSucks();
            collision.gameObject.GetComponent<PlayerHealth>().takeDamage(EnemyAttackDamage);
        }
    }

    protected override void FixedUpdate()
    {
        
    }


    public override void returnToStart()
    {

    }
}
