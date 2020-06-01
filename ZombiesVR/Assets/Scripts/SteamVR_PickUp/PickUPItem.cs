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
        gameObject.GetComponent<MeshCollider>().isTrigger = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        if (gameObject.GetComponent<Magazine>() != null)
        {
            gameObject.GetComponent<Magazine>().timer = 0;
        }
    }
    public void DropObject()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
