using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
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
    private void playButtonSound()
    {

    }
}
