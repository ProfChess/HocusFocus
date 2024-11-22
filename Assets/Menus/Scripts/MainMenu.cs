using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Sounds
    [SerializeField] private AudioSource buttonSound;

    //Menus
    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject SoundMenu;

    //Buttons
    //Start
    public void startButton()
    {
        playButtonSound();
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

    private void playButtonSound()
    {
        buttonSound.Play();
    }

    private void changeMenu(GameObject obj)
    {
        if (obj != null)
        {
            if (obj == StartMenu)
            {
                SoundMenu.SetActive(false);
            }
            if (obj == SoundMenu)
            {
                StartMenu.SetActive(false);
            }
            obj.SetActive(true);
        }

    }
}
