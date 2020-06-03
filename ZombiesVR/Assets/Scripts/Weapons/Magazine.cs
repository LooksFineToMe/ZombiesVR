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
        if (other.gameObject.CompareTag("Belt") && timer >= .4f)
        {
            if (m_Interactable.attachedToHand == true)
            {
                m_Interactable.attachedToHand.DetachObject(magazine);
            }
            gameObject.transform.parent = other.gameObject.transform;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            for (int i = 0; i < gameObject.GetComponentsInChildren<BoxCollider>().Length; i++)
            {
                gameObject.GetComponentsInChildren<BoxCollider>()[0].isTrigger = true;
            }
            
        }
    }
}
