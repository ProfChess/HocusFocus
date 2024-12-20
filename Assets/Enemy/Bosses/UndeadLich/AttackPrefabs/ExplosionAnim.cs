using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnim : MonoBehaviour
{
    public void ouch()
    {
        GetComponentInParent<BA_Explosion>().turnCollider();
        AudioManager.Instance.playBossSound("Explosion");
    }
    public void goAway()
    {
        GetComponentInParent<BA_Explosion>().endAttacking();
    }
}
