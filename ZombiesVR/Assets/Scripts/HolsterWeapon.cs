using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HolsterWeapon : MonoBehaviour
{
    public bool weaponHolstered;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (weaponHolstered == false && other.gameObject.CompareTag("Weapon"))
        {
            other.gameObject.GetComponent<Interactable>().attachedToHand.DetachObject(other.gameObject);
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.transform.parent = gameObject.transform;
            other.gameObject.transform.position = gameObject.transform.position + offset;
            foreach (Transform trans in other.gameObject.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = LayerMask.NameToLayer("PickedUpObject");
            }
            weaponHolstered = true;
        }
    }
}
