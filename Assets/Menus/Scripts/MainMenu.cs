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
    //Start
    public void startButton()
    {
        playButtonSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartingRoom");
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
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.getGameOver())
            {
                changeMenu(EndMenu.gameObject);

            }
        }
    }
}
