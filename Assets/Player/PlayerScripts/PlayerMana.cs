using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public float maxMana = 20f;
    private float pMana = 20f;
    private float manaRegenRate = 2;


    private void Start()
    {
        setMaxMana();
        pMana = GameManager.Instance.savedMana;
        UIManager.Instance.updateManaBar(pMana, maxMana);
    }

    private void Update()
    {   
        //Check if mana is under max, then increase mana over time
        if (pMana < maxMana)
        {
            pMana += manaRegenRate * Time.deltaTime;
            GameManager.Instance.savedMana = pMana; 
            if (pMana > maxMana)
            {
                pMana = maxMana;
            }
        }
        UIManager.Instance.updateManaBar(pMana, maxMana);
    }

    private void setMaxMana()
    {
        maxMana = 20 + (GameManager.Instance.getManaUpgrades() * 10);
    }

    public void decreaseMana(float amount)
    {
        pMana -= amount;
    }

    public void pickupMana()
    {
        setMaxMana();
        pMana = maxMana;
    }

    public float returnCurrentMana()
    {
        return pMana;
    }

    public float getMana()
    {
        return pMana;
    }

    public void setMana(float amount)
    {
        pMana = amount;
    }

}
