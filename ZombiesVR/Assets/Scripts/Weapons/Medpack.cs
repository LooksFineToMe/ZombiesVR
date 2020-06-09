using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Medpack : MonoBehaviour
{
    public int healthRegen;
    bool used;
    bool isUsing;
    public SteamVR_Action_Boolean healAction;
    private Interactable interactable;
    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }
    public void RegainHealth()
    {

    }
    private void Update()
    {
        if (interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;
            if (healAction[source].state)
            {
                isUsing = true;
            }
            
        }
        else
        {
            isUsing = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && used == false && isUsing == true)
        {
            used = true;
            other.GetComponentInParent<PlayerStats>().health += healthRegen;
            Destroy(gameObject);
        }
    }
}
