using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    public float enemyHealth = 5f;
    private float enemyDamageMod = 1f;

    //dot variables
    private bool isDotActive = false;
    private float dotDamage = 0f;
    private float dotDuration = 0f;
    private float dotInterval = 1f;

    private void Start()
    {
        gameObject.SetActive(true);
    }
    public void takeDamage(float damage)
    {
        enemyHealth -= damage * enemyDamageMod;
        if (enemyHealth <= 0)
        {
            dyingSucks();
        }
    }

    public void dyingSucks()
    {
        gameObject.SetActive(false);
        gameObject.GetComponent<BaseEnemyMovement>().returnToStart();
    }



    public void applyDamageMod(float amount, float duration)
    {
        enemyDamageMod = amount;
        Invoke("regularDamageMod", duration);
    }

    public void applyDot(float damage, float duration, float interval)
    {
        dotDuration = duration;
        dotDamage = damage;
        dotInterval = interval;

        if (!isDotActive)
        {
            StartCoroutine(DOT());
        }
    }

    private void regularDamageMod()
    {
        enemyDamageMod = 1f;
    }

    //Damage over time coroutine
    private IEnumerator DOT()
    {
        isDotActive = true;
        float timePassed = 0f;

        while (timePassed < dotDuration)
        {
            takeDamage(dotDamage);
            yield return new WaitForSeconds(dotInterval);
            timePassed += dotInterval;
        }

        isDotActive = false;
    }

}
