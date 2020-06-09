using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUPItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PickUpObject()
    {
        gameObject.layer = LayerMask.NameToLayer("PickedUpObject");
        if (gameObject.GetComponent<MeshCollider>() != null) { gameObject.GetComponent<MeshCollider>().isTrigger = false; }
        //if (gameObject.GetComponent<BoxCollider>() != null) { gameObject.GetComponent<BoxCollider>().isTrigger = true;}
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        if (gameObject.GetComponent<Magazine>() != null)
        {
            gameObject.GetComponent<Magazine>().timer = 0;
        }
    }
    public void PickUpMedKit()
    {
        if (gameObject.GetComponent<MeshCollider>() != null) { gameObject.GetComponent<MeshCollider>().isTrigger = false; }
        if (gameObject.GetComponent<BoxCollider>() != null) { gameObject.GetComponent<BoxCollider>().isTrigger = true; }
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
    public void DropObject()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        if (gameObject.GetComponent<MeshCollider>() != null) { gameObject.GetComponent<MeshCollider>().isTrigger = false; }
        if (gameObject.GetComponent<BoxCollider>() != null) { gameObject.GetComponent<BoxCollider>().isTrigger = false; }
    }
}
