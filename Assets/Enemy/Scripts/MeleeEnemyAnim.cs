using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAnim : MonoBehaviour
{
    public MeleeEnemy enemy;
    public Animator enemyAnim;
    public EnemyHealthScript enemyHP;
    public void callAttack()
    {
        enemy.enemyAttackNumbers();
    }

    public void enemyDeath()
    {
        enemyHP.dyingSucks();
    }

    public void enemyHPCheck()
    {
        if (enemyHP)
        {
            if (enemyHP.enemyHealth <= 0)
            {
                enemyAnim.SetTrigger("Death");
            }
        }

    }
}
