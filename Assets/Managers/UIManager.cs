using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Instance -> Persist between scenes
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    public Image healthBar;
    public Image manaBar;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void loadUITasks()
    {
        if (GameManager.Instance.isCollected("HealthUpgrade01"))
        {
            OptionalTask1.SetActive(false);
        }
        if (GameManager.Instance.isCollected("ManaUpgrade01"))
        {
            OptionalTask2.SetActive(false);
        }
        if (GameManager.Instance.isCollected("DashUpgrade"))
        {
            Task1.SetActive(false);
        }
        if (GameManager.Instance.isCollected("JumpUpgrade"))
        {
            Task2.SetActive(false);
        }
        if (GameManager.Instance.isCollected("TeleportUpgrade"))
        {
            Task3.SetActive(false);
        }
    }

    //Player UI
    public void updateHealthBar(float currentHealth, float maxHealth) //Changes Healthbar to match current health
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }
    public void updateManaBar(float currentMana, float maxMana) //Changes Manabar to match current health
    {
        if (manaBar != null)
        {
            manaBar.fillAmount = currentMana / maxMana;
        }
    }

    //Sound
    private void playUISound()
    {
        AudioManager.Instance.getButtonSound().Play();
    }

    //Pause Menu
    [Header("Pause Menu")]
    [SerializeField] private GameObject PauseMenu;

    //Fast Travel
    [Header("Fast Travel")]
    [SerializeField] private GameObject FastTravelMenu;
    [SerializeField] private FastTravelManager FTManager;
    [SerializeField] private List<GameObject> TravelButtons;

    //UI
    [Header("UI")]
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject HowToPlayMenu;

    //Objectives
    [Header("Objective List")]
    public GameObject Task1;
    public GameObject Task2;
    public GameObject Task3;
    public GameObject Task4;
    [Header("Optional")]
    public GameObject OptionalTask1;
    public GameObject OptionalTask2;
    public void activateUI()
    {
        playerUI.SetActive(true);
    }
    public void deactivateUI()
    {
        playerUI.SetActive(false);
    }

    public void pauseGame()
    {
        playUISound();
        PauseMenu.SetActive(true);
        FastTravelMenu.SetActive(false);
        HowToPlayMenu.SetActive(false);
        Time.timeScale = 0f;
    }
    public void UnpauseGame()
    {
        playUISound();
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void openFTMenu()
    {
        playUISound();
        PauseMenu.SetActive(false);
        FastTravelMenu.SetActive(true);
        for (int i = 0; i < TravelButtons.Count; i++)
        {
            if (FTManager.isUnlocked(FTManager.FastTravel[i].SceneName))
            {
                TravelButtons[i].SetActive(true);
            }
            else
            {
                TravelButtons[i].SetActive(false);
            }
        }
    }
    public void QuitGame()
    {
        playUISound();
        GameManager.Instance.saveOnExit();
        GameManager.Instance.FadingOutTransition();
        UnpauseGame();
        Invoke("WrapMenuLoad", 1f);
        PauseMenu.SetActive(false);
        playerUI.SetActive(false);
    }
    private void WrapMenuLoad()
    {
        SceneManager.LoadScene("Menus");
    }

    //Fast Travel Menu
    public void travelTo(string name)
    {
        playUISound();
        GameManager.Instance.fastTravel(name);
        GameManager.Instance.FadeToBlack(name);
        FastTravelMenu.SetActive(false);
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    //How To Play Menu
    public void openControlMenu()
    {
        playUISound();
        HowToPlayMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }
}
