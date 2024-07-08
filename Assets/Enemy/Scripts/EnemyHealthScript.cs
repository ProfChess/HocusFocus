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

    //Anim
    public Animator enemyAnim;

    private void Start()
    {
        gameObject.SetActive(true);
        enemyAnim = gameObject.GetComponentInChildren<Animator>();
    }
    public void takeDamage(float damage)
    {
        enemyHealth -= damage * enemyDamageMod;
        enemyAnim.SetTrigger("Hurt");
    }

    public void dyingSucks()
    {
        gameObject.SetActive(false);
        StopCoroutine(DOT());
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
