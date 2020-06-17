using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Magazine : MonoBehaviour
{
    public string weaponType = "RenameThis!";//in the inspector we need to rename this the the tag for the reload pivot
    public int magCount = 8;
    public int maxAmmo = 8;
    public GameObject magazine;
    Interactable m_Interactable;
    [HideInInspector]
    public float timer;

    public GameObject[] bullets;

    private void Awake()
    {
        m_Interactable = GetComponent<Interactable>();
    }
    private void Start()
    {
        for (int i = 0; i < magCount; i++)
        {
            bullets[i].SetActive(true);
        }
        if (magCount <= 0)
        {
            Destroy(gameObject, 4);
        }
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }
    public void Detachobject() 
    {
        m_Interactable.attachedToHand.DetachObject(magazine);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(weaponType) && other.GetComponent<ReloadPoint>().magInGun == false && timer > .3f)
        {
            
            other.gameObject.GetComponent<ReloadPoint>().ReloadGun(magCount);
            if (m_Interactable.attachedToHand == true)
            {
                m_Interactable.attachedToHand.DetachObject(magazine);
            }
            Destroy(gameObject);
        }
        //if (other.gameObject.CompareTag("Belt") && timer >= .3f)
        //{
        //    if (m_Interactable.attachedToHand == true)
        //    {
        //        m_Interactable.attachedToHand.DetachObject(magazine);
        //    }
        //    gameObject.transform.parent = other.gameObject.transform;
        //    gameObject.GetComponent<Rigidbody>().isKinematic = true;

        //    foreach (Transform trans in gameObject.GetComponentsInChildren<Transform>(true))
        //    {
        //        trans.gameObject.layer = LayerMask.NameToLayer("PickedUpObject");
        //    }
        //}

        //if (other.gameObject.CompareTag("Belt") && timer >= .3f && magCount == maxAmmo)
        //{
        //    if (m_Interactable.attachedToHand == true)
        //    {
        //        m_Interactable.attachedToHand.DetachObject(magazine);
        //    }
            //gameObject.transform.parent = other.gameObject.transform;
            //gameObject.GetComponent<Rigidbody>().isKinematic = true;
            //foreach (Transform trans in gameObject.GetComponentsInChildren<Transform>(true))
            //{
            //    trans.gameObject.layer = LayerMask.NameToLayer("PickedUpObject");
            //}

    }
}

