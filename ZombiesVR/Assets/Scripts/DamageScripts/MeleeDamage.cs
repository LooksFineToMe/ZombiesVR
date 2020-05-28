using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    Rigidbody rb;
    public float minimumVelocity = 0.05f;
    public int axeDamage = 3;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Enemy")) { collision.gameObject.GetComponent<AIZombie>().TakePlayerDamage(); }
        if (rb.velocity.magnitude >= minimumVelocity && collision.gameObject.CompareTag("Enemy")) 
        { collision.gameObject.GetComponent<EnemyBodyParts>().DamageBodyPart(axeDamage); }
    }
    private void Update()
    {
        print(rb.velocity);
        
    }
}
