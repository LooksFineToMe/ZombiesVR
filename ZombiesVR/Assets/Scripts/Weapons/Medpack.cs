using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Medpack : MonoBehaviour
{
    public Material medpackDefault;
    public Material medpackActive;
    public int healthRegen;
    bool used = false;
    bool isUsing;
    public SteamVR_Action_Boolean healAction;
    private Interactable interactable;
    public float timer;
    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }
    public void RegainHealth()
    {

    }
    private void Update()
    {
        if (timer <= 1)
        {
            timer += Time.deltaTime;
        }
        if (interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;
            if (healAction[source].state)
            {
                isUsing = true;
                gameObject.GetComponent<Renderer>().material = medpackActive;
            }
            else
            {
                isUsing = false;
                gameObject.GetComponent<Renderer>().material = medpackDefault;
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && used == false && isUsing == true)
        {
            used = true;
            other.GetComponentInParent<PlayerStats>().HealPlayer();
            Destroy(gameObject);
        }
        //if (other.gameObject.CompareTag("Belt") && timer >= .3f)
        //{
        //    if (interactable.attachedToHand == true)
        //    {
        //        interactable.attachedToHand.DetachObject(gameObject);
        //    }
        //    gameObject.transform.parent = other.gameObject.transform;
        //    gameObject.GetComponent<Rigidbody>().isKinematic = true;

        //    foreach (Transform trans in gameObject.GetComponentsInChildren<Transform>(true))
        //    {
        //        trans.gameObject.layer = LayerMask.NameToLayer("PickedUpObject");
        //    }
        //}
    }
}
