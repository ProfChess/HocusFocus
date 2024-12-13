using System.Collections.Generic;
using UnityEngine;

public class BA_CombineAttacks : BaseAttackSpawn
{
    [SerializeField] List<BaseAttackSpawn> FirstAttackList;
    [SerializeField] List<BaseAttackSpawn> SecondAttackList;

    private BossController SavedBoss;
    //Called from phase 3 script 
    public override void executeAttack(BossController boss)
    {
        base.executeAttack(boss);
        SavedBoss = boss;
    }

    //Returns an attack from a given attack list 
    private BaseAttackSpawn selectAttack(List<BaseAttackSpawn> givenList)
    {
        int ran = Random.Range(0, givenList.Count);
        return givenList[ran];
    }

    public override void SpawnBossAttack()
    {
        //Grab an attack from each list
        BaseAttackSpawn FirstAttack = selectAttack(FirstAttackList);
        BaseAttackSpawn SecondAttack = selectAttack(SecondAttackList);

        //Perform both at once (Work out animations later)
        FirstAttack.SpawnBossAttack();
        SecondAttack.SpawnBossAttack();
    }
}
