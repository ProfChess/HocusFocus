using UnityEngine;

public class BA_LightningStrikeSpawn : BaseAttackSpawn
{
    [SerializeField] private int extraLightningNumber = 2;
    public override void executeAttack(BossController boss)
    {

        GameObject lightning = poolManager.getObjectFromPool(2);
        float time = lightning.GetComponent<BaseBossAttack>().getAttackSpeed();

        if (lightning != null)
        {
            findPlayer();
            lightning.GetComponent<BA_LightningStrike>().Initialize(poolManager, playerLocation.position, time, true);
        }

        //Extra Lightning
        for (int i = 0; i < extraLightningNumber; i++) //Create extra lightning strike and either spawn it behind or ahead by a random amount
        {
            GameObject ExtraLightning = poolManager.getObjectFromPool(2);
            if (ExtraLightning != null)
            {
                float xdiff = Random.Range(3, 6);
                Vector2 newSpawn = playerLocation.transform.position;
                if (i == 0)
                {
                    newSpawn = new Vector2(playerLocation.position.x + xdiff, playerLocation.position.y);
                }
                else if (i == 1)
                {
                    newSpawn = new Vector2(playerLocation.position.x - xdiff, playerLocation.position.y);
                }
                
                ExtraLightning.GetComponent<BA_LightningStrike>().Initialize(poolManager, newSpawn, time, true);
            }
        }


    }
}
