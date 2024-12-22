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

    protected override void removeUITask()
    {
        UIManager.Instance.Task1.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        if (GameManager.Instance.isCollected(itemID))
        {
            UIManager.Instance.Task1.SetActive(false);
        }
    }
}
