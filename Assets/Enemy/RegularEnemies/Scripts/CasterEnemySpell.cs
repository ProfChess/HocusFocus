using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEnemySpell : MonoBehaviour
{
    //Spell Stats
    private float spellDamage = 3f;
    private float spellSpeed = 5f;
    private float spellLength = 10f;
    private float xSpawn;
    public Vector2 spellDirection = Vector2.right;

    //Collision
    private bool wasHit = false;

    //Animation
    public SpriteRenderer sr;
    private Animator spellAnim;
    private void Start()
    {
        xSpawn = transform.position.x;
        spellAnim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        //Move over time
        if (wasHit)
        {
            spellHit();
        }
        else if (gameObject.transform.position.x >= (xSpawn + spellLength) || gameObject.transform.position.x
            <= (xSpawn - spellLength))
        {
            deactivateSpell();
        }
        else
        {
            transform.Translate(spellDirection * spellSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player") && !wasHit)
            {
                collision.gameObject.GetComponent<PlayerHealth>().takeDamage(spellDamage);
                wasHit = true;
            }
        }
    }

    public void setSpellDirection(Vector2 spellCast)
    {
        spellDirection = spellCast;
        if (spellDirection == Vector2.right)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    private void spellHit()
    {
        spellAnim.SetTrigger("Hit");
        Invoke("deactivateSpell", 1);
    }

    private void deactivateSpell()
    {
        gameObject.SetActive(false);
        Invoke("endSelf", 1);
    }


    private void endSelf()
    {
        Destroy(gameObject);
    }
}
