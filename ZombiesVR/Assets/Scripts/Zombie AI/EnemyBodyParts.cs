using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyParts : MonoBehaviour
{
    public AIZombie aiZombie;
    public int extraDamage;
    public bool isDetachable;
    [Tooltip("The Particle effects to instantiate on detatch")]
    public GameObject bloodParticle;
    public GameObject particlePos;
    [Tooltip("position of the particlePos so we need to change it at run time")]
    public Vector3 bloodOffset;
    [Tooltip("Specify if this body part can apply damage to the player")]
    public bool applyDamage = false;
    public Vector3 detachedScale;
    
    //added "knocked" bool to AI.TAKEPLAYERDAMAGE
    public void DamageBodyPart(int damagesource/*, bool knocked*/)
    {
        aiZombie.TakePlayerDamage(damagesource + extraDamage/*, knocked*/);
        if (isDetachable == true)
        {
            DetachGameObject();
        }
    }

    [ContextMenu("Dismemberment")]
    void DetachGameObject()
    {
        if (applyDamage)
            applyDamage = false;

        gameObject.transform.localScale = detachedScale;
        //to avoid the blood particle effect size being change by the detatch scale we set its position to the parent of the arm
        Instantiate(bloodParticle, gameObject.transform.position + bloodOffset, Quaternion.identity);
        //figure out how to follow the zombies arm

    }

    //doesnt work
    private void OnCollisionEnter(Collision collision)
    {
        print("collided with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponentInParent<PlayerStats>() != null && applyDamage)
        {
            collision.gameObject.GetComponentInParent<PlayerStats>().TakeDamage();
            print("hit " + collision.gameObject.name);
        }
    }
}
