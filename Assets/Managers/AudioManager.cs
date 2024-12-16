using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    //Player
    [SerializeField] private AudioSource playerMove;          //0
    [SerializeField] private AudioSource playerJump;          //1
    [SerializeField] private AudioSource playerDash;          //2
    [SerializeField] private AudioSource playerTeleport;      //3
    [SerializeField] private AudioSource playerHit;           //4
    [SerializeField] private AudioSource playerDeath;         //5
    [SerializeField] private AudioSource playerCast;          //6

    //Enemy
    [SerializeField] private AudioSource enemyDeath;          //7
    [SerializeField] private AudioSource enemySwing;          //8
    [SerializeField] private AudioSource enemyCast;           //9
    [SerializeField] private AudioSource enemyHit;            //14

    //Items
    [SerializeField] private AudioSource itemCollect;         //10

    //Spells
    [SerializeField] private AudioSource playerFireSpell;     //11
    [SerializeField] private AudioSource playerIceSpell;      //12
    [SerializeField] private AudioSource playerThunderSpell;  //13
    [SerializeField] private AudioSource playerComboStart;    //15
    [SerializeField] private AudioSource playerComboStop;     //16
    [SerializeField] private AudioSource playerSpellSelect;   //17
    public void playSound(int soundIndex)
    {
        if (Time.deltaTime == 0f)
        {
            return;
        }
        switch (soundIndex)
        {
            case 0:
                playerMove.PlayDelayed(0.1f);
                playerMove.loop = true;
                break;
            case 1:
                playerJump.Play(); break;
            case 2: 
                playerDash.Play(); break;
            case 3:
                playerTeleport.Play(); break;
            case 4:
                playerHit.Play(); break;
            case 5:
                playerDeath.Play(); break;
            case 6:
                playerCast.Play(); break;
            case 7:
                enemyDeath.Play(); break;
            case 8: 
                enemySwing.Play(); break;
            case 9:
                enemyCast.Play(); break;
            case 10:
                itemCollect.Play(); break;
            case 11:
                playerFireSpell.Play(); break;
            case 12:
                playerIceSpell.Play(); break;
            case 13:
                playerThunderSpell.Play(); break;
            case 14:
                enemyHit.Play(); break;
            case 15:
                playerComboStart.Play(); break;
            case 16:
                playerComboStop.Play(); break;
            case 17:
                playerSpellSelect.Play(); break;
            default:
                Debug.LogWarning(soundIndex + " = Invalid sound index");
                break;

        }
    }

    public bool checkSoundPlaying(int soundIndex)
    {
        switch (soundIndex)
        {
            case 0:
                return playerMove.isPlaying;
            case 1:
                return playerJump.isPlaying;
            case 2:
                return playerDash.isPlaying;
            case 3:
                return playerTeleport.isPlaying;         
            case 4:
                return playerHit.isPlaying;
            case 5:
                return playerDeath.isPlaying;
            case 6:
                return playerCast.isPlaying;
            case 7:
                return enemyDeath.isPlaying;
            case 8:
                return enemySwing.isPlaying;
            case 9:
                return enemyCast.isPlaying;
            case 10:
                return itemCollect.isPlaying;
            case 11:
                return playerFireSpell.isPlaying;
            case 12:
                return playerIceSpell.isPlaying;
            case 13:
                return playerThunderSpell.isPlaying;
            case 14:
                return enemyHit.isPlaying;
            case 15:
                return playerComboStart.isPlaying;
            case 16:
                return playerComboStop.isPlaying;
            case 17:
                return playerSpellSelect.isPlaying;
            default:
                Debug.LogWarning(soundIndex + " = Invalid sound index");
                return false;

        }
    }

    public void stopMovingSound()
    {
        playerMove.loop = false;
    }





}
