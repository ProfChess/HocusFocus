using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float pHealth;
    private float maxHealth = 10;

    private void Start()
    {
        pHealth = maxHealth;
    }

    public void takeDamage(float damage)
    {
        pHealth -= damage;
        if (pHealth <= 0)
        {
            Debug.Log("Player is Dead");
            //Death Logic 
        }
    }



}
