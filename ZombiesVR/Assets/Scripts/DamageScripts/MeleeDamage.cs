using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    Rigidbody rb;
    public float minimumVelocity = 0.05f;
    public int axeDamage = 3;
    public Transform axePos;
    

    [Header("RagDoll Settings")]
    public Rigidbody impactTarget = null;
    public Vector3 impact;
    public float impactTimer;
    public float impactEndTime = 0f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Enemy")) { collision.gameObject.GetComponent<AIZombie>().TakePlayerDamage(); }
        if (rb.velocity.magnitude >= minimumVelocity && collision.gameObject.CompareTag("Enemy")) 
        { 
            collision.gameObject.GetComponent<EnemyBodyParts>().DamageBodyPart(axeDamage);
            impactTarget = gameObject.GetComponent<Rigidbody>();
            impactTimer = 0;
        }
    }
    
    private void Update()
    {
        //TestFunction();
        impactTimer += Time.deltaTime;
        print(rb.velocity);
        RagDollEffect();
    }

    private void TestFunction()
    {
        RaycastHit hit;

        if (Physics.Raycast(axePos.transform.position, axePos.transform.TransformDirection(Vector3.forward), out hit))
        {
            if (hit.rigidbody != null)
            {
                //find the RagdollHelper component and activate ragdolling
                RagdollHelper helper = GetComponent<RagdollHelper>();
                helper.ragdolled = true;

                //set the impact target to whatever the ray hit
                impactTarget = hit.rigidbody;

                //impact direction also according to the ray
                impact = axePos.transform.TransformDirection(Vector3.forward) * 2.0f;

                //the impact will be reapplied for the next 250ms
                //to make the connected objects follow even though the simulated body joints
                //might stretch
                impactEndTime = Time.deltaTime + 0.25f;
            }
        }
    }
    public void RagDollEffect()
    {
        if (Time.deltaTime < impactEndTime && impactTarget != null)
        {
            impactTarget.AddForce(impact, ForceMode.VelocityChange);
        }
    }
}
