using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseItem : MonoBehaviour
{

    private BoxCollider2D hitBox;
    private Vector2 location;
    public string itemName; //Determines what item does on pickup
    public string itemID;   //Determines if item will spawn or not

    protected virtual void Start()
    {
        hitBox = GetComponent<BoxCollider2D>();
        location = transform.position;
        if (GameManager.Instance.isCollected(itemID))
        {
            gameObject.SetActive(false);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && hitBox != null)
        {
            if (collision.CompareTag("Player"))
            {
                //Sound
                AudioManager.Instance.playItemSound("Item");
                //Save
                GameManager.Instance.itemCollected(itemName);
                GameManager.Instance.AddItem(itemID);
                Scene scene = SceneManager.GetActiveScene();
                string sceneName = scene.name;
                GameManager.Instance.saveSceneAndLocation(sceneName, location);
                //Remove Item
                gameObject.SetActive(false);
                
            }
        }
    }


}
