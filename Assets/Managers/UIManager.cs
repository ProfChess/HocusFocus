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


    //Pause Menu
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject FastTravelMenu;
    public void pauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    
    public void openFTMenu()
    {
        PauseMenu.SetActive(false);
        FastTravelMenu.SetActive(true);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Menus");
        PauseMenu.SetActive(false);
    }

}
