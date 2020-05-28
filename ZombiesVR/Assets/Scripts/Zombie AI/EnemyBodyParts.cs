using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyParts : MonoBehaviour
{
    public AIZombie aiZombie;
    public int extraDamage;

    public void DamageBodyPart(int damagesource)
    {
        aiZombie.TakePlayerDamage(damagesource + extraDamage);
    }
}
