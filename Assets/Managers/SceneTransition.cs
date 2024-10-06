using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad = "Null";
    public string spawnPointName = "Null";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If Scene or spawn is set to null -> do nothing
        if (sceneToLoad == "Null" || spawnPointName == "Null")
        {
            Debug.Log("Scene is Null");
        }

        else if (collision.CompareTag("Player"))
        {
            //Fade In
            GameManager.Instance.respawn = false;
            GameManager.Instance.SetSpawnPoint(spawnPointName);
            GameManager.Instance.FadeToBlack(sceneToLoad);
        }
    }
}
