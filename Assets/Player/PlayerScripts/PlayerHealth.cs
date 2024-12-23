using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float pHealth;
    public float maxHealth = 10f;
    public Animator playerAnim;
    public bool playerIsDead;

    //Iframes
    private bool isInvincable = false;

    private void Start()
    {
        playerIsDead = false;
        setMaxHealth();
        pHealth = GameManager.Instance.savedHealth;
        UIChange();     //Changes UI to match health %
    }

    public void takeDamage(float damage)
    {
        if (isInvincable)
        {
            return;
        }
        else
        {
            //Anim and Sound
            playerAnim.SetTrigger("PlayerHit");
            //Damage Logic
            pHealth -= damage;
            GameManager.Instance.saveHealth(pHealth);
            if (pHealth >= 0)
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.playPlayerSound("Hit");
                }
            }
            if (pHealth <= 0 && !playerIsDead)
            {
                AudioManager.Instance.playPlayerSound("Death");
                //Anim
                playerAnim.SetTrigger("PlayerDeath");
                GameManager.Instance.respawn = true;
                playerIsDead=true;
                //Death Logic 

            }
            UIChange();
            StartCoroutine(IFrames());
        }

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

    public void restoreHealth(int amount)
    {
        pHealth += amount;
        GameManager.Instance.saveHealth(pHealth);
        if (pHealth > maxHealth)
        {
            pHealth = maxHealth;
            GameManager.Instance.saveHealth(pHealth);
        }
        UIChange();
    }

    //Update UI 
    public void UIChange()
    {
        UIManager.Instance.updateHealthBar(pHealth, maxHealth);
    }


    //Iframes
    private IEnumerator IFrames()
    {
        isInvincable = true;
        yield return new WaitForSeconds(1f);
        isInvincable = false;
    }
}
