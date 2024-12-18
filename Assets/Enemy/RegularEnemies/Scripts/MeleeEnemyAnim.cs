using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAnim : MonoBehaviour
{
    public MeleeEnemy enemy;
    public Animator enemyAnim;
    private EnemyHealthScript enemyHP;

    private void Start()
    {
        enemyHP = GetComponentInParent<EnemyHealthScript>();
    }
    public void callAttack()
    {
        enemy.enemyAttackNumbers();
        AudioManager.Instance.playEnemySound("Swing");
    }

    public void enemyDeath()
    {
        enemyHP.dyingSucks();
    }

    public void enemyHPCheck()
    {
        if (enemyHP.getEnemyCurrentHealth() <= 0)
        {
            enemyAnim.SetTrigger("Death");
        }
    }
}
