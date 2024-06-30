using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    public float maxMana = 20;
    public float pMana;
    private float manaRegenRate = 2;

    private void Start()
    {
        pMana = maxMana;
    }

    private void Update()
    {   
        //Check if mana is under max, then increase mana over time
        if (pMana < maxMana)
        {
            pMana += manaRegenRate * Time.deltaTime;
            if (pMana > maxMana)
            {
                pMana = maxMana;
            }
        }
    }

    public void decreaseMana(float amount)
    {
        pMana -= amount;
    }

    public void increaseMana(float amount)
    {
        maxMana += amount;
        pMana = maxMana;
    }

    public float returnCurrentMana()
    {
        return pMana;
    }

}
