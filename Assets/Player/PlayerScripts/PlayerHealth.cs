using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float pHealth;
    private float maxHealth = 10;
    public Animator playerAnim;

    

    private void Start()
    {
        pHealth = maxHealth;
        UIManager.Instance.updateHealthBar(pHealth, maxHealth);
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
        UIManager.Instance.updateHealthBar(pHealth, maxHealth);
    }

    public void increaseHealth(float amount)
    {
        maxHealth += amount;
        pHealth = maxHealth;
    }


}
