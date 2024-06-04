using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallDamage : BaseSpellDamage
{
    
    protected void Awake()
    {
        xSpawn = gameObject.transform.position.x;
        spellLength = 10f;
        spellSpeed = 10f;
        spellDamage = 5f;
        effectArea = new Vector2(2.0f, 2.0f);
        col = GetComponent<BoxCollider2D>();
        spellHit = false;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        gameObject.GetComponentInChildren<Transform>().localScale = effectArea;
        
    }






}
