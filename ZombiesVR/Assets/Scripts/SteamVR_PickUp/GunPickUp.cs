using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class GunPickUp : MonoBehaviour
{
    public Interactable interactable;
    public GameObject leftHand;
    public GameObject rightHand;
    // Start is called before the first frame update
    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }
    public void PickUpGun()
    {
        //SteamVR_Input_Sources inputSource;
        //if (inputSource.LeftHand) { leftHand.SetActive(true); }
        //else if (interactable.attachedToHand.handType == SteamVR_Input_Sources.RightHand) { rightHand.SetActive(true); }
    }
    public void DropGun()
    {
        leftHand.SetActive(false);
        rightHand.SetActive(false);
    }
}
