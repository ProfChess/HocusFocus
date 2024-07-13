using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEnemyAnim : MonoBehaviour
{
    public EnemyHealthScript enemyHP;
    public Animator enemyAnim;
    public CasterEnemy enemy;

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

    public void spellCall()
    {
        enemy.cultistSpellLogic();
    }

}
