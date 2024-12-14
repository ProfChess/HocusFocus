using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    //Bools To Start/End Fight
    private bool BossFightStarted = false;  //Starts Fight
    private bool BossFightFinished = false; //Ends Fight
    private bool startedOnce = false;       //Only starts fight once
    private bool finishedOnce = false;      //Finishes fight once

    private bool isFading = false;          //Fading alpha of sprites
    private float fadeSpeed = 2f;           //Speed of fade

    //Trigger Location of Boss
    [SerializeField] private Transform startLocation;        //Place Player must walk to start fight

    //Close Door
    [SerializeField] private BoxCollider2D DoorBox;
    [SerializeField] private GameObject DoorVisual;
    [SerializeField] private List<SpriteRenderer> DoorTrees;

    //Boss Reference
    private BossController boss;

    private void Awake()
    {
        boss = FindObjectOfType<BossController>();
    }
    private void Start()
    {
        BossFightFinished = false; BossFightStarted = false;
        DoorBox.enabled = false;
        foreach (SpriteRenderer SR in DoorTrees)
        {
            Color color = SR.color;
            color.a = 0;
            SR.color = color;
        }
        
    }

    private void Update()
    {
        //Trigger For Player Location
        if (GameManager.Instance.player.transform.position.x >= startLocation.position.x)
        {
            BossFightStarted = true;
        }


        if (BossFightStarted && !BossFightFinished && !startedOnce)
        {
            boss.BeginFight();
            startedOnce = true;
            isFading = true;
            DoorBox.enabled = true;
            //Start Coroutine to Close Door, Spawn Boss, etc
        }
        if (BossFightFinished && !finishedOnce)
        {
            finishedOnce = true;
            //Trigger End Screen + Unlock Door
        }
        if (isFading)
        {
            IncreaseAlpha();
        }
    }

    //Change Alpha To Close Door Visually
    private void IncreaseAlpha()
    {
        foreach (SpriteRenderer SR in DoorTrees)
        {
            Color color = SR.color;
            color.a = Mathf.MoveTowards(color.a, 1f, fadeSpeed * Time.deltaTime);
            SR.color = color;
        }
        if (Mathf.Approximately(DoorTrees[0].color.a, 1f))
        {
            isFading = false;
        }
    }
}
