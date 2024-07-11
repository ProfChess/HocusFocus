using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSummoner : MonoBehaviour
{
    public EnemyHealthScript enemyHP;
    public Animator enemyAnim;
    public SummonerEnemy enemy;

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

    public void summonCall()
    {
        string summonType = enemy.summonType;
        if (summonType == "Single")
        {
            enemy.basicSummonAbility();
        }
        if (summonType == "Double")
        {
            enemy.runSummonAbility();
        }
    }
}
