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
    public GameObject playerPrefab;

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
    //Save Locations
    private string sceneDeath = "StartingRoom";
    public Vector3 respawnLocation = new Vector3(7.75f, -0.75f, 1);
    public bool respawn = false;

    //Item Storage
    private List<string> itemsCollected;
    
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
        itemsCollected = new List<string>();
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
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Time.timeScale = 0;
        respawnLogic();
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


    //Get/Save Health/Mana
    //Get
    public float getHealthUpgrades() {  return healthUpgrades; }
    public float getManaUpgrades() {  return manaUpgrades; }

    //Save Stats
    public void saveHealthAndMana() 
    {
        savedHealth = player.GetComponent<PlayerHealth>().getHealth();
        savedMana = player.GetComponent<PlayerMana>().getMana();
    }
    //Set player stats on entering new room
    private void transferPlayerStats()
    {
        player.GetComponent<PlayerHealth>().setHealth(savedHealth);
        player.GetComponent<PlayerMana>().setMana(savedMana);
    }
    //Save Scene/Location For respawning after death
    public void saveSceneAndLocation(string sceneName)
    {
        sceneDeath = sceneName;
    }
    private void respawnLogic()
    {
        SceneManager.LoadScene(sceneDeath);
        
        if (player == null)
        {
            player = Instantiate(playerPrefab, respawnLocation, Quaternion.identity);
        }
        else
        {
            Destroy(player);
            player = Instantiate(playerPrefab, respawnLocation, Quaternion.identity);
        }
        Time.timeScale = 1;
    }



    //Get/Set Item
    public bool getDashBool() { return dashUpgrade; }
    public bool getJumpBool() { return jumpUpgrade; }
    public bool getTeleportBool() { return teleportUpgrade; }

    public void setDashBool() { dashUpgrade = true; player.GetComponent<PlayerController>().playerDash = true; }
    public void setJumpBool() {  jumpUpgrade = true; player.GetComponent<PlayerController>().playerDoubleJump = true; }
    public void setTeleportBool() {  teleportUpgrade = true; player.GetComponent<PlayerController>().playerTeleport = true; }


    //Item Storage
    //Add Item
    public void AddItem(string item)
    {
        if (item != null && !itemsCollected.Contains(item))
        {
            itemsCollected.Add(item);
        }
    }

    public bool isCollected(string item)
    {
        return itemsCollected.Contains(item);
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

}
