using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : BaseItem
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth.pickupHealth();
        }
    }
}
