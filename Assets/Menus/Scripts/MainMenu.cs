using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Sounds
    [SerializeField] private AudioSource buttonSound;

    //Menus
    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject SoundMenu;
    [SerializeField] private GameObject EndMenu;

    //Buttons
    [SerializeField] private GameObject NewGameButton;
    [SerializeField] private GameObject ContinueButton;

    //UI
    [SerializeField] private GameObject playerUI;

    //Start
    public void startButton()
    {
        playButtonSound();
        Time.timeScale = 1f;
        GameManager.Instance.FadingOutTransition();
        Invoke("beginGame", 1f);
        
    }
    private void beginGame()
    {
        GameManager.Instance.loadProgress();
    }
    public void exitGame()
    {
        playButtonSound();
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void goSoundMenu()
    {
        playButtonSound();
        changeMenu(SoundMenu);
    }

    public void goStartMenu()
    {
        playButtonSound();
        changeMenu(StartMenu);
    }

    public void backFromSound()
    {
        saveSoundSettings();
        goStartMenu();
    }

    public void goEndMenu()
    {
        playButtonSound();
        changeMenu(EndMenu);
    }

    private void playButtonSound()
    {
        if (buttonSound == null)
        {
            buttonSound = AudioManager.Instance.getButtonSound();
        }
        buttonSound.Play();
    }

    private void changeMenu(GameObject obj)
    {
        if (obj != null)
        {
            if (obj == StartMenu)
            {
                SoundMenu.SetActive(false);
                EndMenu.SetActive(false);
            }
            if (obj == SoundMenu)
            {
                StartMenu.SetActive(false);
                EndMenu.SetActive(false);
            }
            if (obj == EndMenu)
            {
                SoundMenu.SetActive(false);
                StartMenu.SetActive(false);
            }
            obj.SetActive(true);
        }

    }

    private void Update()
    {


    }

    private void Start()
    {
        GameManager.Instance.FadeToColor();
        GameManager.Instance.respawn = true;
        playerUI.SetActive(false);
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.getGameOver())
            {
                changeMenu(EndMenu.gameObject);

            }
        }
        bool iscontinue = SaveManager.SaveExists();
        ContinueButton.gameObject.SetActive(iscontinue);
        NewGameButton.gameObject.SetActive(!iscontinue);
    }

    private void saveSoundSettings()
    {
        AudioSliderScript SliderCode = GetComponent<AudioSliderScript>();
        float musicVol = SliderCode.getMusicVolume();
        float sfxVol = SliderCode.getSFXVolume();
        PlayerPrefs.SetFloat("MusicVolume", musicVol);
        PlayerPrefs.SetFloat("SFXVolume", sfxVol);
        PlayerPrefs.Save();
    }
}
