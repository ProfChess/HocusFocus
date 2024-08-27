using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public string spawnPointName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Fade In
            GameManager.Instance.FadeToBlack();
            GameManager.Instance.SetSpawnPoint(spawnPointName);
            GameManager.Instance.loadScene(sceneToLoad);
        }
    }
}
