using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BA_PatternLightningSpawn : BaseAttackSpawn
{
    //Boss Teleport Spawn
    [SerializeField] Vector2 bossPosition;
    [SerializeField] Vector2 spawnPosition;
    //Variables
    private int spawnPattern;
    private bool canCastAgain = true;
    //SpawnPoints
    private float xSpawn1 = 11;
    private float xSpawn2;
    private float xSpawn3 = 5;

    //List of lightning
    private List<GameObject> Strikes;
    [SerializeField] GameObject BGStrike;

    //Reference
    [SerializeField] SpriteRenderer BossVisual;
    private BoxCollider2D hitbox;
    private BossController MainBoss;

    public override void executeAttack(BossController boss)
    {
        if (MainBoss == null || hitbox == null)
        {
            MainBoss = boss;
            hitbox = MainBoss.GetComponent<BoxCollider2D>();
        }

        if (canCastAgain)
        {
            duration = 20f;
            bossLeave();
            StartCoroutine(LoopAttack());
        }
        else
        {
            duration = 0f;
            return;
        }
    }

    //Selects random number, then changes location of middle stike according to number chosen
    //Then creates 3 strikes -> adds to list -> spawns strikes -> starts movement
    private void selectRandomSpawnPattern()
    {
        spawnPattern = Random.Range(1, 3);
        switch (spawnPattern)
        {
            case 1:
                xSpawn2 = 8f;
                break;
            case 2:
                xSpawn2 = 9.5f;
                break;
            case 3:
                xSpawn2 = 6.5f;
                break;
            default:
                Debug.LogWarning("Invalid Spawn Pattern: " +  spawnPattern);
                break;
        }

        //Grab Objects
        for (int i = 0; i < 3; i++)
        {
            GameObject strike = poolManager.getObjectFromPool(3);
            if (strike != null)
            {
                Strikes.Add(strike);
            }
            spawnLightning(strike, i);
        }
        foreach (GameObject strike in Strikes)
        {
            strike.GetComponent<BA_MovingLightning>().setMoveBool();
        }
    }

    //Spawns 3 moving lightning strikes at 3 different locations
    private void spawnLightning(GameObject lightning, int spawnNum)
    {
        BA_MovingLightning ML = lightning.GetComponent<BA_MovingLightning>();
        switch (spawnNum)
        {
            case 0:
                ML.Initialize(poolManager, new Vector2(xSpawn1, -1));
                break;
            case 1:
                ML.Initialize(poolManager, new Vector2(xSpawn2, -1));
                break;
            case 2:
                ML.Initialize(poolManager, new Vector2(xSpawn3, -1));
                break;
        }
    }

    //Teleports boss to side of room (Will be called from anim event)
    private void bossLeave()
    {
        StartCoroutine(teleport(0));
    }

    private void bossComeBack()
    {
        StartCoroutine(teleport(1));
    }

    //Disappears -> Moves boss -> reappears after time given
    private IEnumerator teleport(int moveDir)
    {
        BossSwitchInvuln();
        if (moveDir == 0)
        {
            canCastAgain = false;
            BossVisual.sortingOrder = -16;
            MainBoss.transform.position = bossPosition;
            yield return new WaitForSeconds(1f); //Travel delay
            BossVisual.sortingOrder = 0;
            BGLightning();
            yield return new WaitForSeconds(2f); //Short delay before striking (Background lightning appears here)
        }
        else if (moveDir == 1)
        {
            BossVisual.sortingOrder = -16;
            MainBoss.transform.position = spawnPosition;
            yield return new WaitForSeconds(1f);
            BossVisual.sortingOrder = 0;
            canCastAgain = true;
        }

    }

    //Boss Invuln
    private void BossSwitchInvuln()
    {
        if (hitbox != null)
        {
            hitbox.enabled = !hitbox.enabled;
        }
    }

    //Attack 3 Times 
    private IEnumerator LoopAttack()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 3; i++)
        {
            Strikes = new List<GameObject>();
            selectRandomSpawnPattern();
            yield return new WaitForSeconds(duration / 3);
        }
        bossComeBack();
    }


    //BackGround Lightning 
    private void BGLightning()
    {
        for (float i = 1; i < 6; i++)
        {
            GameObject BGgetstrike = poolManager.getObjectFromPool(2);
            BGgetstrike.GetComponent<BA_LightningStrike>().Initialize(poolManager, new Vector2(3.5f + (i * 1.5f), -1), duration, false);
        }
    }
}
