using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    Rigidbody rb;
    [Tooltip("The minimum Velocity the player has to swing to deal the extra damage Type")]
    public float extraVelocity = 4f;

    public float minimumVelocity = 1f;

    [Tooltip("The base damage of the weapon")]
    public float baseDamage = 3;

    [Tooltip("The base force power of the weapon")]
    public float baseForce = 30f;

    [Tooltip("The body part damage of the weapon")]
    public int bodypartDamage;

    [Tooltip("Is this a blunt weapon or not?")]
    public bool isBluntWeapon;

    [Header("RagDoll Settings")]
    //[HideInInspector]
    public Rigidbody impactTarget = null;//Uncomment hide in inpector when we clean the null referneces

    float timer;//The timer is to ensure we don't double hit the enemy. Cooldown.
    float cooldown = 0.2f;//cooldown for the timer; 

    public TrailRenderer weaponTrail;
    public float weaponTrailwidth = .1f;

    public GameObject soundKnockHit;
private void Start()
    {
        rb = GetComponent<Rigidbody>();//Get the rigibody on startup
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))//The initial check for the enemy hit 
        {
            ContactPoint contactPoint = collision.contacts[0];//Saves the contact point for adding force postion

            //Checks if the collided object has 1.Rigibody, 2. AIZombie Class, 
            if (collision.rigidbody != null && collision.rigidbody.GetComponentInParent<AIZombie>() != null)
            {
                //Creates a zombie variable to access the zombie ragdoll
                AIZombie zombie = collision.rigidbody.GetComponentInParent<AIZombie>();

                //Creates an impactTarget variable for the collision to add force to.
                impactTarget = collision.rigidbody;

                //Checks if the collided object has 1.RagDolled = false, 2.Velocity > ExtraVelocity
                if (collision.rigidbody.GetComponentInParent<AIZombie>().m_RH.ragdolled == false && rb.velocity.magnitude >= extraVelocity)
                {
                    //Starts the coroutine in AIZombie Class
                    StartCoroutine(zombie.ZombieRagdoll());

                    //Adds the damage to the EnemyBodyParts class
                    collision.rigidbody.GetComponent<EnemyBodyParts>().DamageBodyPart(baseDamage);
                    //zombie.m_ScoreManager.ScorePopUp(contactPoint.normal, (int)baseDamage); //maybe not

                    //Adds the force to the other gameObject
                    impactTarget.AddForce(-contactPoint.normal * (baseForce * rb.velocity.magnitude), ForceMode.VelocityChange);
                }

                //Checks if the collided object has 1. Weapon Velocity > MinimumVelocity, 2. Velocity < Extra Velocity, 3. timer > cooldown, 4. IsBluntWeapon = true.
                else if (rb.velocity.magnitude > minimumVelocity && rb.velocity.magnitude < extraVelocity && timer >= cooldown && isBluntWeapon == true)
                {
                    //Resets timer to 0
                    timer = 0;

                    //Starts the corotine to ragdoll the zombie.
                    StartCoroutine(zombie.ZombieRagdoll());

                    //Adds the force to the other gameObject
                    impactTarget.AddForce(-contactPoint.normal * (baseForce * rb.velocity.magnitude), ForceMode.VelocityChange);
                    
                    //Adds the damage to the enemyBodyparts class
                    collision.rigidbody.GetComponent<EnemyBodyParts>().BluntDamage(baseDamage);

                    if (soundKnockHit != null) { Instantiate(soundKnockHit, transform.position, transform.rotation); }
                }

                //Checks if the collided object has 1. Weapon Velocity > MinimumVelocity, 2. Velocity < Extra Velocity, 3. timer > cooldown, 4. IsBluntWeapon = false. 
                else if (rb.velocity.magnitude > minimumVelocity && rb.velocity.magnitude < extraVelocity && timer >= .2f && isBluntWeapon == false)
                {
                    //Resets timer to 0
                    timer = 0;

                    //Adds the damage to the enemy bodypart class
                    collision.rigidbody.GetComponent<EnemyBodyParts>().Stagger(baseDamage, bodypartDamage);
                }
                impactTarget = null;//Clears the impact target
            }
        }
    }
    
    private void Update()
    {
        //Checks if the timer is above 1
        if (timer <= 1) { timer += Time.deltaTime; }
        //print("Weapon Velocity is: " + rb.velocity.magnitude);
        if (weaponTrail != null)
        {
            if (rb.velocity.magnitude > minimumVelocity && rb.velocity.magnitude <= extraVelocity)
            {
                weaponTrail.startColor = Color.white;
                weaponTrail.startWidth = weaponTrailwidth;
            }
            else if (rb.velocity.magnitude > extraVelocity)
            {
                weaponTrail.startColor = Color.red;
            }
            else
            {
                weaponTrail.startWidth = Mathf.Lerp(weaponTrail.startWidth, 0, 10 * Time.deltaTime);
            }
        }
        
    }
}
