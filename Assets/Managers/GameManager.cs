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
    private float fadeDuration = 0.5f;

    //CheckPoints and Saving
    //Stats
    private float healthUpgrades = 0f;
    private float manaUpgrades = 0f;
    private float savedHealth = 10f;
    private float savedMana = 20f;
    //Items
    private bool dashUpgrade = false;
    private bool jumpUpgrade = false;
    private bool teleportUpgrade = false;
    
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
        transferPlayerStats();
    }

    public void SetSpawnPoint(string spawnPoint)
    {
        currentSpawnPoint = spawnPoint;
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

        transferPlayerStats();
    }

    //End of Game
    public void playerDeath()
    {
        player.GetComponent<PlayerController>().playerSpeed = 0;
        Time.timeScale = 0;
    }

    //Fade to Black
    public void FadeToBlack(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
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

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        saveHealthAndMana();
        float timePassed = 0f;
        while (timePassed < fadeDuration)
        {
            timePassed += Time.deltaTime;
            float alphaValue = Mathf.Clamp01(timePassed / fadeDuration);
            SetAlpha(alphaValue);
            yield return null;
        }

        SetAlpha(1f);
        SceneManager.sceneLoaded += onSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void SetAlpha(float alphaValue)
    {
        if(blackFade != null)
        {
            Color color = blackFade.color;
            color.a = alphaValue;
            blackFade.color = color;
        }
    }


    //Item Collection
    public void itemCollected(string itemName)
    {
        //Stats
        if (itemName == "HealthUpgrade")
        {
            healthUpgrades += 1;
        }
        if (itemName == "ManaUpgrade")
        {
            manaUpgrades += 1;
        }

        //Items
        if (itemName == "Dash")
        {
            dashUpgrade = true;
        }
        if (itemName == "Jump")
        {
            jumpUpgrade = true;
        }
        if (itemName == "Teleport")
        {
            teleportUpgrade = true;
        }

    }

    //Get/Save Health/Mana
    //Get
    public float getHealthUpgrades() {  return healthUpgrades; }
    public float getManaUpgrades() {  return manaUpgrades; }

    //Save
    public void saveHealthAndMana() 
    {
        savedHealth = player.GetComponent<PlayerHealth>().getHealth();
        savedMana = player.GetComponent<PlayerMana>().getMana();
    }

    private void transferPlayerStats()
    {
        player.GetComponent<PlayerHealth>().setHealth(savedHealth);
        player.GetComponent<PlayerMana>().setMana(savedMana);
    }

    //Get/Set Item
    public bool getDashBool() { return dashUpgrade; }
    public bool getJumpBool() { return jumpUpgrade; }
    public bool getTeleportBool() { return teleportUpgrade; }

    public void setDashBool() { dashUpgrade = true; player.GetComponent<PlayerController>().playerDash = true; }
    public void setJumpBool() {  jumpUpgrade = true; player.GetComponent<PlayerController>().playerDoubleJump = true; }
    public void setTeleportBool() {  teleportUpgrade = true; player.GetComponent<PlayerController>().playerTeleport = true; }




}
