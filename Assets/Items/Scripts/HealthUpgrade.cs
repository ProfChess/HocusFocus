using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : BaseItem
{
    public float healthIncrease = 10;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth.increaseHealth(healthIncrease);
        }
    }
}
