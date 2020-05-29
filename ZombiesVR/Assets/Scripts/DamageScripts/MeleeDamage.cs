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
            collision.gameObject.GetComponent<EnemyBodyParts>().DamageBodyPart(axeDamage/*, false*/);     //added "knocked" bool to AI.TAKEPLAYERDAMAGE and DAMAGEBODYPART
            //impactTarget = collision.gameObject.GetComponent<Rigidbody>();
            //impactTimer = 0;
        }
    }
    
    private void Update()
    {
        TestFunction();
        impactTimer += Time.deltaTime;
        print(rb.velocity);
        RagDollEffect();
        impact = axePos.localPosition;
    }

    private void TestFunction()
    {
        RaycastHit hit;

        if (Physics.Raycast(axePos.transform.position, axePos.transform.TransformDirection(Vector3.forward), out hit, 1))
        {
            if (hit.rigidbody != null && hit.rigidbody.GetComponentInParent<RagdollHelper>() != null)
            {
                print(hit.collider.name);
                //find the RagdollHelper component and activate ragdolling

                RagdollHelper helper = hit.rigidbody.GetComponentInParent<RagdollHelper>();
                //print(hit.collider.GetComponentInParent<RagdollHelper>().ToString());
                helper.ragdolled = true;
                helper.anim = hit.rigidbody.GetComponentInParent<Animator>();
                //we need to find a way to set the animator of the object being hit


                //set the impact target to whatever the ray hit
                impactTarget = hit.rigidbody;

                //impact direction also according to the ray
                //impact = axePos.transform.TransformDirection(Vector3.forward) * 2.0f;
                impact = axePos.transform.TransformDirection(Vector3.forward) * 2.0f;
                //the impact will be reapplied for the next 250ms
                //to make the connected objects follow even though the simulated body joints
                //might stretch
                impactEndTime = Time.deltaTime + .25f;

            }
        }
        if (Time.deltaTime < impactEndTime && impactTarget != null)
        {
            impactTarget.AddForce(impact, ForceMode.VelocityChange);
        }
    }
    public void RagDollEffect()
    {
        
    }
}
