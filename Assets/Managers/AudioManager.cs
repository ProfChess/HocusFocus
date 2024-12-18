using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SoundFX
{
    public string SoundName;
    public AudioSource SoundSource;
}
public class AudioManager : MonoBehaviour
{

    //Instance to be kept between scenes
    private static AudioManager AudioManagerInstance;
    public static AudioManager Instance { get { return AudioManagerInstance; } }

    private void Awake()
    {
        if (AudioManagerInstance == null)
        {
            AudioManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    //Sound Effects
    //List
    [SerializeField] public List<SoundFX> PlayerSoundList;
    [SerializeField] public List<SoundFX> EnemySoundList;
    [SerializeField] public List<SoundFX> ItemSoundList;
    [SerializeField] public List<SoundFX> SpellSoundList;
    [SerializeField] public List<SoundFX> BossSoundList;


    //Player
    [Header("Player")]
    [SerializeField] private AudioSource playerMove;          

    //Music
    [Header("Music")]
    [SerializeField] private AudioSource MainMenuMusic;
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource BossMusic;

    //UI 
    [SerializeField] private AudioSource ButtonSound;

    public bool checkSoundPlaying()
    {
        return playerMove.isPlaying;
    }

    public void stopMovingSound()
    {
        playerMove.loop = false;
    }
    public void startMovingSound()
    {
        playerMove.loop = true;
        playerMove.Play();
    }

    public void changeMusic(int MusicIndex)
    {
        stopAllMusic();
        switch (MusicIndex)
        {
            case 0: //Main Menu
                MainMenuMusic.Play(); break;
            case 1: //Background
                BGM.Play(); break;
            case 2: //Boss Fight Music
                BossMusic.Play(); break;    
            default:
                Debug.Log("Music Not Found");
                break;
        }
    }

    private void stopAllMusic()
    {
        MainMenuMusic.Stop();
        BGM.Stop();
        BossMusic.Stop();
    }

    private void Update()
    {
        if (MainMenuMusic != null && BGM != null && BossMusic != null) 
        {
            Scene curScene = SceneManager.GetActiveScene();
            if (curScene.name == "Menus")
            {
                if (!MainMenuMusic.isPlaying)
                {
                    changeMusic(0);
                }
            }
            else if (curScene.name == "Room_19_Boss_1")
            {
                if (!BossMusic.isPlaying)
                {
                    changeMusic(2);
                }
            }
            else
            {
                if (!BGM.isPlaying)
                {
                    changeMusic(1);
                }
            }
        }
    }

    public AudioSource getButtonSound()
    {
        return ButtonSound;
    }

    public void playPlayerSound(string name)
    {
        for (int i = 0; i < PlayerSoundList.Count; i++)
        {
            if (PlayerSoundList[i].SoundName == name)
            {
                PlayerSoundList[i].SoundSource.Play();
            }
        }
    }
    public void playEnemySound(string name)
    {
        for (int i = 0; i < EnemySoundList.Count; i++)
        {
            if (EnemySoundList[i].SoundName == name)
            {
                EnemySoundList[i].SoundSource.Play();
            }
        }
    }
    public void playItemSound(string name)
    {
        for (int i = 0; i < ItemSoundList.Count; i++)
        {
            if (ItemSoundList[i].SoundName == name)
            {
                ItemSoundList[i].SoundSource.Play();
            }
        }
    }
    public void playSpellSound(string name)
    {
        for (int i = 0; i < SpellSoundList.Count; i++)
        {
            if (SpellSoundList[i].SoundName == name)
            {
                SpellSoundList[i].SoundSource.Play();
            }
        }
    }

    public void playBossSound(string name)
    {
        for (int i = 0; i < BossSoundList.Count; i++)
        {
            if (BossSoundList[i].SoundName == name)
            {
                BossSoundList[i].SoundSource.Play();
            }
        }
    }



}
