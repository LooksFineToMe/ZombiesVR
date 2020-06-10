using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    public float destroyTime = 2;
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
