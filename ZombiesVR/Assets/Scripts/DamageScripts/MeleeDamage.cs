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
    public bool isBluntWeapon;

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
        
        if (collision.gameObject.CompareTag("Enemy")) 
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
                if (collision.rigidbody.GetComponentInParent<RagdollHelper>().ragdolled == false && rb.velocity.magnitude >= minimumVelocity)
                {
                    helper.ragdolled = true;
                    collision.rigidbody.GetComponent<EnemyBodyParts>().DamageBodyPart(axeDamage);
                    //impactTarget.AddForce(axePos.transform.forward * (axeForce * rb.velocity.magnitude), ForceMode.VelocityChange);
                    impactTarget.AddForce(-contactPoint.normal * (axeForce * rb.velocity.magnitude), ForceMode.VelocityChange);
                    impactTarget = null;
                }
                else if (rb.velocity.magnitude > 1f && rb.velocity.magnitude < minimumVelocity && collision.gameObject.CompareTag("Enemy") && timer >= .2f && isBluntWeapon == true)
                {
                    timer = 0;
                    helper.ragdolled = true;
                    impactTarget.AddForce(-contactPoint.normal * (axeForce * rb.velocity.magnitude), ForceMode.VelocityChange);
                    collision.rigidbody.GetComponent<EnemyBodyParts>().BluntDamage(axeDamage);
                }
                else if (rb.velocity.magnitude > 1f && rb.velocity.magnitude < minimumVelocity && collision.gameObject.CompareTag("Enemy") && timer >= .2f && isBluntWeapon == false)
                {
                    timer = 0;
                    collision.rigidbody.GetComponent<EnemyBodyParts>().Stagger(axeDamage, bodypartDamage);
                }
            }
            
        }
        //REMEMBER TO RE_ENABLE THIS
        
        
    }
    
    private void Update()
    {
        timer += Time.deltaTime;
        print(rb.velocity);
    }
}
