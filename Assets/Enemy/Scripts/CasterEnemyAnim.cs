using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEnemyAnim : MonoBehaviour
{
    private EnemyHealthScript enemyHP;
    public Animator enemyAnim;
    public CasterEnemy enemy;

    private void Start()
    {
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

    public void spellCall()
    {
        enemy.cultistSpellLogic();
    }

}
