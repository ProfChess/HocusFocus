using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class SpellAnim : MonoBehaviour
{
    public void spellDeath()
    {
        gameObject.GetComponentInParent<BaseSpellDamage>().turnOff();
    }

    
    public void playBolt()
    {
        gameObject.GetComponentInParent<ArcaneSpellDamage>().playNextBolt();
    }

    public void firstBolt()
    {
        gameObject.GetComponentInParent<ArcaneSpellDamage>().playFirstBolt();
    }
}
