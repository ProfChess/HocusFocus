using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneCloak : BaseItem
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().playerDash = true;
            GameManager.Instance.setDashBool();
        }
    }
}
