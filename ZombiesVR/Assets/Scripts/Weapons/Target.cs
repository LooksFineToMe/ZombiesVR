using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Animator m_Animations;
    private AudioSource m_AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_Animations = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("projectile"))
        {
            m_Animations.SetTrigger("Hit");
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
        }
    }

    [ContextMenu("Hit")]
    private void Function()
    {
        m_Animations.SetTrigger("Hit");
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
    }
}
