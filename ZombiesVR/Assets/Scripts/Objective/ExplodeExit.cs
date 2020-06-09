using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeExit : MonoBehaviour
{
    public WinManager win;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("projectile"))
        {
            win.ExplodeExit();
        }
        
    }
}
