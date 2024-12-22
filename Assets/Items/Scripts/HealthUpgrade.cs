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
            GameManager.Instance.savedHealth = playerHealth.getHealth();
            playerHealth.pickupHealth();
        }
    }
    protected override void removeUITask()
    {
        UIManager.Instance.OptionalTask1.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        if (GameManager.Instance.isCollected(itemID))
        {
            UIManager.Instance.OptionalTask1.SetActive(false);
        }
    }
}
