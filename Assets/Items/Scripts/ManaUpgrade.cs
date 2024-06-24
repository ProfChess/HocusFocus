using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaUpgrade : BaseItem
{
    public float manaIncrease = 10;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            PlayerMana playerMana = collision.GetComponent<PlayerMana>();
            playerMana.increaseMana(manaIncrease);

        }
    }
}
