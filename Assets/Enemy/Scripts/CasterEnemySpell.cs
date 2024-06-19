using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEnemySpell : MonoBehaviour
{
    private float spellSpeed = 5f;
    public Vector2 baseSize = new Vector2(1f, 1f);
    public Vector2 maxSize = new Vector2(10f, 1f);
    public Vector2 spellDirection = Vector2.right;

    private BoxCollider2D spellArea;
    private Vector2 currentSize;
    private Vector3 initialPosition;

    //Spell Damage
    private float spellDamage = 2f;
    private bool wasHit = false;

    private void Start()
    {
        spellArea = GetComponent<BoxCollider2D>();
        currentSize = baseSize;
        initialPosition = transform.position;
        transform.localScale = new Vector3(currentSize.x, currentSize.y, 1f);
        spellArea.size = currentSize;

        if (spellDirection == Vector2.left)
        {
            transform.localScale = new Vector3(-currentSize.x, currentSize.y, 1f);
        }
    }

    private void Update()
    {
        //Increase in size over time
        if (currentSize.x < maxSize.x)
        {
            float growth = spellSpeed * Time.deltaTime;
            currentSize.x += growth;
            if (spellDirection == Vector2.right)
            {
                transform.position = initialPosition + new Vector3(currentSize.x / 2, 0, 0);
            }
            else if (spellDirection == Vector2.left)
            {
                transform.position = initialPosition - new Vector3(currentSize.x / 2, 0, 0);
            }
            transform.localScale = new Vector3(currentSize.x, currentSize.y, 1f);
            spellArea.size = new Vector2(currentSize.x, currentSize.y); 
        }

        if(currentSize.x >= maxSize.x)
        {
            deactivateSpell();
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
