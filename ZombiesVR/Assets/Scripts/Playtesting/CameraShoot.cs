using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShoot : MonoBehaviour
{

    public float impactForce = 2;
    public bool knock = false;

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
            if (Physics.Raycast(ray, out hit))
            {
                //check if the raycast target has a rigid body (belongs to the ragdoll)
                if (hit.rigidbody != null)
                {
                    //find the RagdollHelper component and activate ragdolling
                    RagdollHelper helper = hit.rigidbody.GetComponentInParent<RagdollHelper>();
                    AIZombie ai = hit.rigidbody.GetComponentInParent<AIZombie>();
                    helper.ragdolled = true;

                    //set the impact target to whatever the ray hit
                    impactTarget = hit.rigidbody;

                    //impact direction also according to the ray
                    impact = ray.direction * impactForce;
                    ai.TakePlayerDamage(10);

                    //the impact will be reapplied for the next 250ms
                    //to make the connected objects follow even though the simulated body joints
                    //might stretch
                    impactEndTime = Time.deltaTime + 2f;
                }
            }
        }

        if (Time.deltaTime < impactEndTime && hit.rigidbody != null)
        {
            impactTarget.AddForce(impact, ForceMode.Impulse);
        }
    }
}
