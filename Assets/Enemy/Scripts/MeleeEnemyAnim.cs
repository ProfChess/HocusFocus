using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAnim : MonoBehaviour
{
    public MeleeEnemy enemy;
    public Animator enemyAnim;
    private EnemyHealthScript enemyHP;
    public void callAttack()
    {
        enemy.enemyAttackNumbers();
        enemyHP = GetComponentInParent<EnemyHealthScript>();
        enemyHP.onHealthChanged += enemyHPCheck;
    }

    public void enemyDeath()
    {
        enemyHP.dyingSucks();
    }

    public void enemyHPCheck(float health)
    {
        if (health <= 0)
        {
            enemyAnim.SetTrigger("Death");
        }
    }
}
