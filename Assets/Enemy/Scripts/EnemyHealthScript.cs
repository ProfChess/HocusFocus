using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    public float enemyHealth = 5f;

    public void takeDamage(float damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            dyingSucks();
        }
    }

    public void dyingSucks()
    {
        gameObject.SetActive(false);
    }

}
