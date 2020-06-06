using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class GunPickUp : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;
    // Start is called before the first frame update
    private void Start()
    {

    }
    public void PickUpGun()
    {
        if (gameObject.GetComponentInParent<Hand>().handType == SteamVR_Input_Sources.LeftHand)
        {
            leftHand.SetActive(true);
        }
        if (gameObject.GetComponentInParent<Hand>().handType == SteamVR_Input_Sources.RightHand)
        {
            rightHand.SetActive(true);
        }
    }
    public void DropGun()
    {
        leftHand.SetActive(false);
        rightHand.SetActive(false);
    }
}
