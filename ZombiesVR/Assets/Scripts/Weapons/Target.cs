using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Animator m_Animations;

    // Start is called before the first frame update
    void Start()
    {
        m_Animations = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Bullet"))
        {
            m_Animations.SetTrigger("Hit");
        }
    }
}
