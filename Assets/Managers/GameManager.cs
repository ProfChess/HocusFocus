using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private string currentSpawnPoint;
    public GameObject player;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Start()
    {
    }

    public void SetSpawnPoint(string spawnPoint)
    {
        currentSpawnPoint = spawnPoint;
    }

    public void loadScene(string sceneName)
    {
        SceneManager.sceneLoaded += onSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= onSceneLoaded;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null && !string.IsNullOrEmpty(currentSpawnPoint))
        {
            GameObject spawnPoint = GameObject.Find(currentSpawnPoint);
            if (spawnPoint != null)
            {
                player.transform.position = spawnPoint.transform.position;
            }
        }
    }

    //End of Game
    public void playerDeath()
    {
        Time.timeScale = 0;
    }

}
