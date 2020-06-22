using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProperties : MonoBehaviour
{
    [Header("BulletProperties")]
    public float timeToDestroy;
    public float bulletDamage = 1;
    public int bodypartDamage = 1;
    public bool heavy;

    [Header("RagDollPhysics")]
    public Transform bulletPos;
    public Rigidbody impactTarget;
    public float bulletForce = 10;
    public GameObject particle;
    public GameObject bloodSplat;
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contactPoint = collision.contacts[0];
        if (collision.rigidbody != null && collision.rigidbody.GetComponentInParent<RagdollHelper>() != null && heavy == false)
        {
            
            //find the RagdollHelper component and activate ragdolling

            RagdollHelper helper = collision.rigidbody.GetComponentInParent<RagdollHelper>();
            //print(hit.collider.GetComponentInParent<RagdollHelper>().ToString());

            helper.anim = collision.rigidbody.GetComponentInParent<Animator>();
            //we need to find a way to set the animator of the object being hit


            //set the impact target to whatever the ray hit
            impactTarget = collision.rigidbody;

            collision.rigidbody.GetComponent<EnemyBodyParts>().Stagger(bulletDamage, bodypartDamage);

            impactTarget = null;
            //impact direction also according to the ray
            //impact = axePos.transform.TransformDirection(Vector3.forward) * 2.0f;
            //to make the connected objects follow even though the simulated body joints
            //might stretch
            if (collision.rigidbody.GetComponentInParent<RagdollHelper>().ragdolled == false)
            {
                //IF WE ADD GUN KNOCK DOWN HERE IS WHERE WE ADD IT
                //helper.ragdolled = true;
                //collision.rigidbody.GetComponent<EnemyBodyParts>().Stagger(bulletDamage);
                //impactTarget.AddForce(-contactPoint.normal * (bulletForce), ForceMode.VelocityChange);
                //impactTarget.AddForce(bulletPos.transform.forward * bulletForce, ForceMode.VelocityChange);
            }
        }
        else if (collision.rigidbody != null && collision.rigidbody.GetComponentInParent<RagdollHelper>() != null && heavy == true)
        {

            //find the RagdollHelper component and activate ragdolling

            RagdollHelper helper = collision.rigidbody.GetComponentInParent<RagdollHelper>();
            //print(hit.collider.GetComponentInParent<RagdollHelper>().ToString());

            helper.anim = collision.rigidbody.GetComponentInParent<Animator>();
            //we need to find a way to set the animator of the object being hit


            //set the impact target to whatever the ray hit
            impactTarget = collision.rigidbody;

            //collision.rigidbody.GetComponent<EnemyBodyParts>().Stagger(bulletDamage, bodypartDamage);

            impactTarget = null;
            collision.rigidbody.GetComponentInParent<AIZombie>().PowerDeath();
        }
    }
}
