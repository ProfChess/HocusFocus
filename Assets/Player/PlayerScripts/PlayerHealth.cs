using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float pHealth = 10f;
    public float maxHealth = 10f;
    public Animator playerAnim;

    private void Start()
    {
        setMaxHealth();
        pHealth = GameManager.Instance.savedHealth;
        UIChange();     //Changes UI to match health %
    }

    public void takeDamage(float damage)
    {
        //Anim
        playerAnim.SetTrigger("PlayerHit");
        //Damage Logic
        pHealth -= damage;
        GameManager.Instance.saveHealth(pHealth);
        if (pHealth <= 0)
        {
            Debug.Log("Player is Dead");
            //Anim
            playerAnim.SetTrigger("PlayerDeath");
            GameManager.Instance.respawn = true;
            //Death Logic 
            
        }
        UIChange();
    }

    private void setMaxHealth()
    {
        maxHealth = 10 + (GameManager.Instance.getHealthUpgrades() * 10);
    }

    public void pickupHealth()
    {
        setMaxHealth();
        pHealth = maxHealth;
        UIChange();
    }

    public float getHealth()
    {
        return pHealth;
    }

    public void setHealth(float amount)
    {
        pHealth = amount;
    }

    //Update UI 
    public void UIChange()
    {
        UIManager.Instance.updateHealthBar(pHealth, maxHealth);
    }

}
