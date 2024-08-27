using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private string currentSpawnPoint;
    public GameObject player;

    //Scene Change
    public Image blackFade;
    private float fadeDuration = 1f;
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
        StartCoroutine(FadeIn());
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
        StartCoroutine(FadeIn());

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
        player.GetComponent<PlayerController>().playerSpeed = 0;
        Time.timeScale = 0;
    }

    //Fade to Black
    public void FadeToBlack()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float timePassed = 0f;
        while (timePassed < fadeDuration)
        {
            timePassed += Time.deltaTime;
            float alphaValue = Mathf.Clamp01(timePassed / fadeDuration);
            SetAlpha(1f - alphaValue);
            yield return null;
        }

        SetAlpha(0f);
    }

    private IEnumerator FadeOut()
    {
        float timePassed = 0f;
        while (timePassed < fadeDuration)
        {
            timePassed += Time.deltaTime;
            float alphaValue = Mathf.Clamp01(timePassed / fadeDuration);
            SetAlpha(alphaValue);
            yield return null;
        }

        SetAlpha(1f);

    }

    private void SetAlpha(float alphaValue)
    {
        Color color = blackFade.color;
        color.a = alphaValue;
        blackFade.color = color;
    }

}
