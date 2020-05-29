using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProperties : MonoBehaviour
{
    public float timeToDestroy;
    public int bulletDamage = 1;
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) { collision.gameObject.GetComponent<EnemyBodyParts>().DamageBodyPart(bulletDamage/*, false*/); }
        Destroy(gameObject);
    }
}
