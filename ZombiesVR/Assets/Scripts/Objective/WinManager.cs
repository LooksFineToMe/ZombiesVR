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
    // Start is called before the first frame update
    void Start()
    {
        m_DefaultMaterial = gameObject.GetComponent<Renderer>().material;
        gameObject.GetComponent<Renderer>().material = m_HighlightedMaterial;
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
}
