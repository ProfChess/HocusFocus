using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        //AudioManager.Instance.playSound(18);
    }

    //Pause Menu
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject FastTravelMenu;
    [SerializeField] private FastTravelManager FTManager;
    [SerializeField] private List<GameObject> TravelButtons;
    public void pauseGame()
    {
        playUISound();
        PauseMenu.SetActive(true);
        FastTravelMenu.SetActive(false);
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
        SceneManager.LoadScene("Menus");
        PauseMenu.SetActive(false);
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

}
