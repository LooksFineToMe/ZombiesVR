﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class PlayerBelt : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Magazine"))
        {
            other.gameObject.transform.parent = gameObject.transform;
        }
    }
    */
    // Update is called once per frame
    void FixedUpdate()
    {
        //trying to rotate on the Y based on the players head postion 
        //gameObject.transform.Rotate(0, player.transform.rotation.y, 0);
    }
}
