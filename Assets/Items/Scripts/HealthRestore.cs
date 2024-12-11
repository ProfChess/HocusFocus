using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRestore : MonoBehaviour
{
    [SerializeField] int HealthToRestore = 2;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                gameObject.SetActive(false);
                collision.GetComponent<PlayerHealth>().restoreHealth(HealthToRestore);
                disappear();
            }
        }
    }

    private void disappear()
    {
        PickupManager.Instance.returnHealth(gameObject);
    }

    
}
