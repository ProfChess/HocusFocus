using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public string spawnPointName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.SetSpawnPoint(spawnPointName);
            GameManager.Instance.loadScene(sceneToLoad);
        }
    }
}
