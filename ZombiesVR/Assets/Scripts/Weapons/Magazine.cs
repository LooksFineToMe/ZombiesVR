using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Magazine : MonoBehaviour
{
    public string weaponType = "RenameThis!";//in the inspector we need to rename this the the tag for the reload pivot
    public int magCount = 8;
    public GameObject magazine;
    Interactable m_Interactable;

    private void Awake()
    {
        m_Interactable = GetComponent<Interactable>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(weaponType))
        { 
            other.gameObject.GetComponent<ReloadPoint>().ReloadGun(magCount);
            m_Interactable.attachedToHand.DetachObject(magazine);
            magazine.SetActive(false);
        
        }
    }
}
