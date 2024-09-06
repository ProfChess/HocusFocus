using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{

    private BoxCollider2D hitBox;
    public string itemName;
    protected virtual void Start()
    {
        hitBox = GetComponent<BoxCollider2D>();
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && hitBox != null)
        {
            if (collision.CompareTag("Player"))
            {
                GameManager.Instance.itemCollected(itemName);
                gameObject.SetActive(false);
                Invoke("destroyItem", 1);
            }
        }
    }

    protected virtual void destroyItem()
    {
        Destroy(gameObject);
    }
}
