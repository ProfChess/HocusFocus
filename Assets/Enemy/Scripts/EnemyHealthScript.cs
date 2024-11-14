using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    public float enemyMaxHealth = 5f;
    private float enemyHealth;
    private float enemyDamageMod = 1f;

    //dot variables
    private bool isDotActive = false;
    private float dotDamage = 0f;
    private float dotDuration = 0f;
    private float dotInterval = 1f;

    //Anim
    public Animator enemyAnim;

    //Getter 
    public float getEnemyCurrentHealth()
    {
        return enemyHealth;
    }

    //Events
    public event Action<float> onHealthChanged;
    private void Start()
    {
        enemyHealth = enemyMaxHealth;
        gameObject.SetActive(true);
    }
    public void takeDamage(float damage)
    {
        enemyHealth -= damage * enemyDamageMod;
        if (onHealthChanged != null)
        {
            onHealthChanged.Invoke(enemyHealth);
        }
        if (enemyAnim)
        {
            enemyAnim.SetTrigger("Hurt");
        }
        else
        {
            if (enemyHealth <= 0f)
            {
                dyingSucks();
                Invoke("die", 1);
            }
        }
    }

    public void dyingSucks()
    {
        gameObject.SetActive(false);
        StopCoroutine(DOT());
        if (gameObject.GetComponent<BaseEnemyMovement>() != null)
        {
            gameObject.GetComponent<BaseEnemyMovement>().returnToStart();
        }
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

    private void die()
    {
        Destroy(gameObject);
    }
}
