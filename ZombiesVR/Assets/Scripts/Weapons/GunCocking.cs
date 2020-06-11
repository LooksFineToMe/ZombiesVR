using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GunCocking : MonoBehaviour
{
    private Interactable interactable;
    public Shooting gun;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }
    private void Update()
    {
        //if (interactable.attachedToHand != null)
        //{
        //    interactable.attachedToHand.DetachObject(gameObject);
        //    gun.CockingGun();
        //}
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
