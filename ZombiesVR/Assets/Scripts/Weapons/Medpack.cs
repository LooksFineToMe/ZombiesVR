using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medpack : MonoBehaviour
{
    public int healthRegen;
    bool used;
    public void RegainHealth()
    { 
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && used == false)
        {
            used = true;
            other.GetComponentInParent<PlayerStats>().health += healthRegen;
            Destroy(gameObject);
        }
    }
}
