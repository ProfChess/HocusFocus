using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class FireArcaneDamage : BaseSpellDamage
{
    protected void Awake()
    {
        spellDamage = 15f;
        spellSpeed = 5f;
        spellLength = 15f;
        col = GetComponent<BoxCollider2D>();
        xSpawn = gameObject.transform.position.x;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<EnemyHealthScript>().takeDamage(spellDamage);
            }
        }
    }

    public override void Update()
    {
        if (gameObject.transform.position.x >= (xSpawn + spellLength) || gameObject.transform.position.x
           <= (xSpawn - spellLength))
        {
            turnOff();
            Invoke("killObject", 2f);
        }
        else
        {
            transform.Translate(spellDirection * spellSpeed * Time.deltaTime);
        }
    }
    
}
