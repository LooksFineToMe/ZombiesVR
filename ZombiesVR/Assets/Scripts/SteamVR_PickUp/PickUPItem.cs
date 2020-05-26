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
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    public void DropObject()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
