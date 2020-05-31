using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyParts : MonoBehaviour
{
    public AIZombie aiZombie;
    public int extraDamage;
    public bool isDetachable;
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
    void DetachGameObject()
    {
        gameObject.transform.localScale = detachedScale;
    }
}
