using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    Rigidbody rb;
    public float minimumVelocity = 0.05f;
    public float axeDamage = 3;
    public float axeForce = 30f;
    public int bodypartDamage;

    [Header("RagDoll Settings")]
    public Rigidbody impactTarget = null;
    public Vector3 impact;

    float timer;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (rb.velocity.magnitude >= minimumVelocity && collision.gameObject.CompareTag("Enemy")) 
        {
            ContactPoint contactPoint = collision.contacts[0];
            
            
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contactPoint.normal);

            if (collision.rigidbody != null && collision.rigidbody.GetComponentInParent<RagdollHelper>() != null)
            {
                
                //find the RagdollHelper component and activate ragdolling

                RagdollHelper helper = collision.rigidbody.GetComponentInParent<RagdollHelper>();
                //print(hit.collider.GetComponentInParent<RagdollHelper>().ToString());
                
                helper.anim = collision.rigidbody.GetComponentInParent<Animator>();
                //we need to find a way to set the animator of the object being hit


                //set the impact target to whatever the ray hit
                impactTarget = collision.rigidbody;

                //impact direction also according to the ray
                //impact = axePos.transform.TransformDirection(Vector3.forward) * 2.0f;
                //to make the connected objects follow even though the simulated body joints
                //might stretch
                if (collision.rigidbody.GetComponentInParent<RagdollHelper>().ragdolled == false)
                {
                    helper.ragdolled = true;
                    collision.rigidbody.GetComponent<EnemyBodyParts>().DamageBodyPart(axeDamage);
                    //impactTarget.AddForce(axePos.transform.forward * (axeForce * rb.velocity.magnitude), ForceMode.VelocityChange);
                    impactTarget.AddForce(-contactPoint.normal * (axeForce * rb.velocity.magnitude), ForceMode.VelocityChange);
                }
            }
        }
        //REMEMBER TO RE_ENABLE THIS
        else if (rb.velocity.magnitude > 1f && rb.velocity.magnitude < minimumVelocity && collision.gameObject.CompareTag("Enemy") && timer >= .2f)
        {
            timer = 0;
            collision.rigidbody.GetComponent<EnemyBodyParts>().Stagger(axeDamage, bodypartDamage);
        }
    }
    
    private void Update()
    {
        timer += Time.deltaTime;
        print(rb.velocity);
    }
}
