using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    public bool m_UnlockedCar;
    public bool m_RefilledCar;
    public int m_CurrentlyRefilled;
    public int m_RefillsNeeded = 3;
    public ParticleSystem m_ObjectiveParticle;
    public Material m_HighlightedMaterial;
    Material m_DefaultMaterial;
    public GameObject m_UnlockedItems;
    public Animator animator;
    public ParticleSystem[] explosions;
    public GameObject explodeTrigger;
    // Start is called before the first frame update
    void Start()
    {
        m_DefaultMaterial = gameObject.GetComponent<Renderer>().material;
        gameObject.GetComponent<Renderer>().material = m_HighlightedMaterial;
        m_UnlockedItems.SetActive(false);
        explodeTrigger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckRefills()
    {
        m_CurrentlyRefilled++;
        if (m_ObjectiveParticle != null) { m_ObjectiveParticle.Play(); }
        if (m_CurrentlyRefilled >= m_RefillsNeeded)
        {
            explodeTrigger.SetActive(true);
            m_RefilledCar = true;
            gameObject.GetComponent<Renderer>().material = m_DefaultMaterial;
            CheckObjectives();
        }
    }
    public void CheckObjectives()
    {
        if (m_UnlockedCar == true && m_RefilledCar == true)
        {
            ObjectiveCompleted();
            if (m_ObjectiveParticle != null) { m_ObjectiveParticle.Play(); }
        }
    }

    private void ObjectiveCompleted()
    {
        print("Win Game!, Stick around and Survive");
    }

    public void GameOver()
    {
        print("GameOver, You escaped!");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (m_UnlockedCar == true)
        {
            UnlockCar();
        }
    }

    private void UnlockCar()
    {
        print("OpenCar");
        m_UnlockedItems.SetActive(true);
        animator.SetBool("OpenBoot", true);
    }
    public void ExplodeExit()
    {
        foreach (ParticleSystem explosion in explosions)
        {
            explosion.Play();
        }
        explodeTrigger.SetActive(false);
        print("Animation.Play();");
        print("Sound.Play();");
    }
}
