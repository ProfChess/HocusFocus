using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float pHealth;
    private float maxHealth = 10;
    public Animator playerAnim;

    private void Start()
    {
        pHealth = maxHealth;
    }

    public void takeDamage(float damage)
    {
        //Anim
        playerAnim.SetTrigger("PlayerHit");
        //Damage Logic
        pHealth -= damage;
        if (pHealth <= 0)
        {
            Debug.Log("Player is Dead");
            //Anim
            playerAnim.SetTrigger("PlayerDeath");
            //Death Logic 
        }
    }

    public void increaseHealth(float amount)
    {
        maxHealth += amount;
        pHealth = maxHealth;
    }



}
