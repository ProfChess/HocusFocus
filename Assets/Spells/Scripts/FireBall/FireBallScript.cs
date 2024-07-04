using System.Collections;
using UnityEngine;

public class FireBallScript : BaseSpell
{
    protected override IEnumerator CastSpellRoutine(bool lookingRight)
    {
        yield return base.CastSpellRoutine(lookingRight);
        GameObject fireball = Instantiate(spellPrefab, spawnPoint.position, Quaternion.identity);

        StartCoroutine(FireBallMove(fireball));
    }

    private IEnumerator FireBallMove(GameObject fireball)
    {   
        float count = 0;
        Vector2 fireDir = direction;
        spellLength = 5;
        while (count < spellLength)
        {
            fireball.transform.Translate(fireDir * speed * Time.deltaTime);
            count += Time.deltaTime;
            yield return null;
        }

        fireballDamage(fireball);

    }


    private void fireballDamage(GameObject fireball)
    {
        Destroy(fireball);
    }

}
