using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProperties : MonoBehaviour
{
    [Header("BulletProperties")]
    public float timeToDestroy;
    public int bulletDamage = 1;

    [Header("RagDollPhysics")]
    public Transform bulletPos;
    public Rigidbody impactTarget;
    public float bulletForce = 10;
    public GameObject particle;
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contactPoint = collision.contacts[0];
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
                collision.rigidbody.GetComponent<EnemyBodyParts>().DamageBodyPart(bulletDamage);
                impactTarget.AddForce(-contactPoint.normal * (bulletForce), ForceMode.VelocityChange);
                //impactTarget.AddForce(bulletPos.transform.forward * bulletForce, ForceMode.VelocityChange);
            }
        }
        if (collision.gameObject.CompareTag("Enemy")) 
        { 
            //collision.gameObject.GetComponent<EnemyBodyParts>().DamageBodyPart(bulletDamage/*, false*/);
            Destroy(gameObject);
            //gameObject.GetComponent<SphereCollider>().enabled = false;
            //particle.SetActive(false);
        }
        
        
    }
}
