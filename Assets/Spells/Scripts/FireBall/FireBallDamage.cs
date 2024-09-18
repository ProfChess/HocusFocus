using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallDamage : BaseSpellDamage
{
    private float areaIncrease; //Gets bigger by this many times on contact
    public Transform visualArea;
    protected void Awake()
    {
        xSpawn = gameObject.transform.position.x;
        spellLength = 10f;
        spellSpeed = 10f;
        spellDamage = 5f;
        areaIncrease = 1.75f;
        col = GetComponent<BoxCollider2D>();
        spellHit = false;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
        visualArea = gameObject.GetComponentInChildren<Transform>();
        visualArea.localScale *= areaIncrease;
        box.size = new Vector2(0.6f, 0.6f);
        box.offset = new Vector2(-0.3f, 0f);
    }






}
