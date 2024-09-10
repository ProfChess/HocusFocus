using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class SpellAnim : MonoBehaviour
{
    //Disable Spell After Complete
    public void spellDeath()
    {
        gameObject.GetComponentInParent<BaseSpellDamage>().turnOff();
    }

    
    //Arcane Visuals
    public void playBolt()
    {
        gameObject.GetComponentInParent<ArcaneSpellDamage>().playNextBolt();
    }

    public void firstBolt()
    {
        gameObject.GetComponentInParent<ArcaneSpellDamage>().playFirstBolt();
    }

    //activate collider at right time
    public void activateArcane()
    {
        gameObject.GetComponentInParent<ArcaneIceDamage>().activateCollider();
    }

    public void activateFire()
    {
        gameObject.GetComponentInParent<FireArcaneDamage>().activateFire();
    }
}
