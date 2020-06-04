using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Keys_Properties : MonoBehaviour
{
    public WinManager m_WinManager;
    Interactable m_Interactable;
    public AudioSource m_KeySound;
    // Start is called before the first frame update
    private void Start()
    {
        m_Interactable = GetComponent<Interactable>();
    }
    public void Update()
    {
        if (m_Interactable.attachedToHand == true)
        {
            Destroy(gameObject, 3);
            print("You found your keys!");
            if (m_KeySound != null && m_WinManager.m_UnlockedCar == false) { PlaySound(); }
            m_WinManager.m_UnlockedCar = true;
        }
    }

    private void PlaySound()
    {
        m_KeySound.Play();
    }
}
