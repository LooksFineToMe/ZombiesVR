using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShoot : MonoBehaviour
{
    public float Range = 20;
    public GameObject m_PlayerHand;
    public float impactForce = 2;
    public bool knock = false;

    public LayerMask rayCastMask;

    float impactEndTime = 0;
    Rigidbody impactTarget = null;
    Vector3 impact;

    Ray ray;
    RaycastHit hit; //a variable that will receive the hit info from the Raycast call below
    // Update is called once per frame
    void Update()
    {
        //if left mouse button clicked
        if (Input.GetMouseButtonDown(0))
        {
            //Get a ray going from the camera through the mouse cursor
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //check if the ray hits a physic collider
            if (Physics.Raycast(ray, out hit, Range, rayCastMask))
            {
                //check if the raycast target has a rigid body(belongs to the ragdoll)
                if (hit.rigidbody != null && hit.rigidbody.GetComponentInParent<RagdollHelper>() != null)
                {
                    //find the RagdollHelper component and activate ragdolling
                    RagdollHelper helper = hit.rigidbody.GetComponentInParent<RagdollHelper>();
                    AIZombie ai = hit.rigidbody.GetComponentInParent<AIZombie>();

                    //set the impact target to whatever the ray hit
                    impactTarget = hit.rigidbody;
                    //impact direction also according to the ray
                    impact = ray.direction * impactForce;

                    //if (hit.rigidbody.GetComponent<EnemyBodyParts>() != null)
                    //{
                    //    EnemyBodyParts Limbs = hit.rigidbody.GetComponent<EnemyBodyParts>();
                    //    Limbs.DamageBodyPart(24.5f);
                    //}

                    ai.TakePlayerDamage(23.3f);

                    ai.Stagger();
                    //hit.rigidbody.GetComponent<EnemyBodyParts>().DamageBodyPart(2);
                    
                    //the impact will be reapplied for the next 250ms
                    //to make the connected objects follow even though the simulated body joints
                    //might stretch
                    impactEndTime = Time.deltaTime + 2f;
                }

                if (hit.collider.tag == "Gun" && hit.collider.GetComponent<PickUPItem>() != null)
                {
                    PickUPItem weapon = hit.collider.GetComponent<PickUPItem>();
                    GrabObject(weapon, true);
                }
                //print(hit.collider.name);
            }
        }
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * Range, Color.blue);

        if (Time.deltaTime < impactEndTime && hit.rigidbody != null)
        {
            impactTarget.AddForce(impact, ForceMode.Impulse);
        }
    }

    private void GrabObject(PickUPItem weapon, bool grabbed)
    {
        Vector3 handPos = m_PlayerHand.transform.position;
        Quaternion handRot = m_PlayerHand.transform.rotation;
        if (grabbed)
        {
            weapon.GetComponent<Rigidbody>().isKinematic = true;
            weapon.transform.parent = m_PlayerHand.transform;
            weapon.transform.position = handPos;
            weapon.transform.rotation = handRot;
        }
        else
        {
            weapon.GetComponent<Rigidbody>().isKinematic = false;
            weapon.transform.parent = weapon.transform;
        }
    }
}
