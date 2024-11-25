using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BA_PatternLightningSpawn : BaseAttackSpawn
{
    //Boss Teleport Spawn
    [SerializeField] Vector2 bossPosition;
    [SerializeField] Vector2 spawnPosition;
    //Variables
    private int spawnPattern;
    //SpawnPoints
    private float xSpawn1 = 10;
    private float xSpawn2;
    private float xSpawn3 = 4;

    //List of lightning
    private List<GameObject> Strikes;

    //Reference
    [SerializeField] SpriteRenderer BossVisual;
    public override void executeAttack(BossController boss)
    {
        Strikes = new List<GameObject>();
        selectRandomSpawnPattern();

    }

    //Selects random number, then changes location of middle stike according to number chosen
    //Then creates 3 strikes -> adds to list -> spawns strikes -> starts movement
    private void selectRandomSpawnPattern()
    {
        spawnPattern = Random.Range(1, 3);
        switch (spawnPattern)
        {
            case 1:
                xSpawn2 = 7f;
                break;
            case 2:
                xSpawn2 = 8.5f;
                break;
            case 3:
                xSpawn2 = 5.5f;
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
        if (moveDir == 0)
        {
            BossVisual.sortingOrder = -16;
            gameObject.transform.position = bossPosition;
            yield return new WaitForSeconds(1f);
            BossVisual.sortingOrder = 0;
        }
        else if (moveDir == 1)
        {
            BossVisual.sortingOrder = -16;
            gameObject.transform.position = spawnPosition;
            yield return new WaitForSeconds(1f);
            BossVisual.sortingOrder = 0;
        }

    }
}
