using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTalisman : BaseItem
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().playerTeleport = true;
        }
    }
}
