using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GunCocking : MonoBehaviour
{
    private Interactable interactable;
    public Interactable gunsInteractable;
    public Shooting gun;
    private BoxCollider cockTrigger;
    public BoxCollider[] pistolColliders;

    private void Start()
    {
        cockTrigger = GetComponent<BoxCollider>();
        interactable = GetComponent<Interactable>();
    }
    private void Update()
    {
        if (gunsInteractable.attachedToHand != null)
        {
            cockTrigger.enabled = true;
            foreach (BoxCollider coliders in pistolColliders)
            {
                coliders.enabled = false;
            }
        }
        else if (gunsInteractable.attachedToHand == null)
        {
            cockTrigger.enabled = false;
            foreach (BoxCollider coliders in pistolColliders)
            {
                coliders.enabled = true;
            }
        }

    }
    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes grabTypes = hand.GetGrabStarting();
        if (interactable.attachedToHand == null && grabTypes != GrabTypes.None && gun.magInGun == true)
        {
            
            gun.CockingGun();

        }
    }
}
