using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private string currentSpawnPoint;
    private string currentSpawnDirection;
    public GameObject player;


    //Respawning
    public GameObject playerPrefab;

    //Scene Change
    public Image blackFade;
    private float fadeDuration = 0.5f;

    //CheckPoints and Saving
    //Stats
    public float healthUpgrades = 0f;
    private float manaUpgrades = 0f;
    public float savedHealth;
    public float savedMana;
    //Items
    private bool dashUpgrade = false;
    private bool jumpUpgrade = false;
    private bool teleportUpgrade = false;
    //Save Locations
    public string sceneDeath = "StartingRoom";
    public Vector2 respawnLocation = new Vector2(7.75f, -0.75f);
    public bool respawn = false;

    //Item Storage
    private List<string> itemsCollected;

    //Saving Between Sessions
    private SaveData saveData;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            itemsCollected = new List<string>();
            saveData = SaveManager.LoadGame() ?? new SaveData
            {
                startingScene = "StartingRoom",
                spawnLocation = new Vector2(7.75f, -0.75f),
                HpUpgrades = 0,
                ManaUpgrades = 0,
                playerItems = itemsCollected,
                dashFound = false,
                jumpFound = false,
                teleFound = false,
            };
            //loadProgress(); //Loads all saved variables to game manager
        }

        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //player.transform.position = saveData.spawnLocation;
        StartCoroutine(FadeIn());
        loadPlayerStart();
    }

    public void SetSpawnPoint(string spawnPoint)
    {
        currentSpawnPoint = spawnPoint;
        if (spawnPoint.Length >= 5)
        {
            string spawnDirection = spawnPoint.Substring(0, 5);
            if (spawnDirection == "Right")
            {
                currentSpawnDirection = spawnDirection;
            }
            else
            {
                spawnDirection = spawnPoint.Substring(0, 4);
                if (spawnDirection == "Left")
                {
                    currentSpawnDirection = spawnDirection;
                }
                else
                {
                    Debug.Log("SpawnPointName Incorrect (Must start with 'Right' or 'Left'");
                }
            }
        }
        else
        {
            Debug.Log("SpawnPointName too short");
        }

    }


    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
        StartCoroutine(FadeIn());
        PickupManager.Instance.returnAllHealth();
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
                if (currentSpawnDirection == "Right") //If spawnPointname starts with 'Right' (meaning we are entering from the right) -> flip to the left
                {
                    player.GetComponentInChildren<SpriteRenderer>().flipX = true;
                    player.GetComponent<PlayerController>().setMoveDirection(Vector2.left);
                }
            }
        }
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
    public void saveHealth(float hp) 
    {
        savedHealth = hp;
    }
    public void saveMana(float mana)
    {
        savedMana = mana;
    }
    private void loadPlayerStart()
    {
        savedHealth = 10 + (healthUpgrades * 10);
        savedMana = 20 + (manaUpgrades * 10);
        player.GetComponent<PlayerHealth>().pickupHealth();
        player.GetComponent<PlayerMana>().pickupMana();
    }

    //Save Scene/Location For respawning after death
    public void saveSceneAndLocation(string sceneName, Vector2 location)
    {
        sceneDeath = sceneName;
        respawnLocation = location;
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
        if (itemsCollected != null)
        {
            return itemsCollected.Contains(item);
        }
        else
        {
            return false;
        }
    }

    //Item Collection
    public void itemCollected(string itemName)
    {
        //Stats
        switch (itemName)
        {
            case "HealthUpgrade":
                healthUpgrades += 1;
                break;

            case "ManaUpgrade":
                manaUpgrades += 1;
                break;

            case "Dash":
                dashUpgrade = true;
                saveData.dashFound = true;
                break;

            case "Jump":
                jumpUpgrade = true;
                saveData.jumpFound = true;
                break;

            case "Teleport":
                teleportUpgrade = true;
                saveData.teleFound = true;
                break;

            default:
                Debug.Log("Invalid Item");
                break;
        }

    }


    //Save and Load Game session
    private void saveProgress()
    {
        saveData.startingScene = sceneDeath;
        saveData.spawnLocation = respawnLocation;
        saveData.HpUpgrades = healthUpgrades;
        saveData.ManaUpgrades = manaUpgrades;
        SaveManager.SaveGame(saveData);
    }

    private void loadProgress()
    {
        sceneDeath = saveData.startingScene;
        respawnLocation = saveData.spawnLocation;
        healthUpgrades = saveData.HpUpgrades;
        manaUpgrades = saveData.ManaUpgrades;
        dashUpgrade = saveData.dashFound;
        jumpUpgrade = saveData.jumpFound;
        teleportUpgrade = saveData.teleFound;
        itemsCollected = saveData.playerItems;
        SceneManager.LoadScene(sceneDeath);
    }

    //Starting Place
    public void startingSceneLoad()
    {
        SceneManager.LoadScene(sceneDeath);    }

    //Quit Game
    private void OnApplicationQuit()
    {
        saveProgress();
    }

}
