﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("AI And Player Specific")]
    [SerializeField] List<AIZombie> m_Zombies;
    [HideInInspector] public List<AIZombie> m_LivingZombies;
    [SerializeField] public List<GameObject> m_SpawnLocations;
    [SerializeField] public List<GameObject> m_Players;

    [Header("Waves Specific")]
    [Tooltip("How long between waves and when the next wave begins")]
    [SerializeField] float m_TimeOffset = 10f;
    private float m_NextWave;

    [SerializeField] int m_WaveSpawnValue = 20;
    private float m_CurrentValueOfWave;

    [Tooltip("How difficult the game will be, Higher values will mean harder difficulties")]
    [SerializeField] int m_IncrementValues = 5;

    //assign the targets for the AI
    private List<GameObject> m_Targets;

    private bool m_ReadyForNextWave = false;

    // Start is called before the first frame update
    void Start()
    {
        m_LivingZombies = new List<AIZombie>();    
    }

    // Update is called once per frame
    void Update()
    {
        //if next wave is ready, spawn them enemies
        if (m_ReadyForNextWave && m_NextWave <= Time.time)
        {
            SpawnWave();
            m_CurrentValueOfWave = 0;
        }
        //if all zombies are dead create the next wave to spawn
        if (m_LivingZombies.Count <= 0)
        {
            m_WaveSpawnValue += m_IncrementValues;
            CreateWave();
        }
    }

    private void CreateWave()
    {
        m_NextWave = Time.deltaTime + m_TimeOffset;
        //this could be buggy
        while (m_CurrentValueOfWave < m_WaveSpawnValue)
        {
            AIZombie zombie = Instantiate(m_Zombies[Random.Range(0, m_Zombies.Count - 1)]);
            zombie.transform.parent = gameObject.transform;
            zombie.transform.position = m_SpawnLocations[Random.Range(0, m_SpawnLocations.Count - 1)].transform.position;
            zombie.SetWaveManager(this);
            zombie.m_Spawner = this;
            m_CurrentValueOfWave += zombie.m_WaveValue;
            m_LivingZombies.Add(zombie);
            zombie.gameObject.SetActive(false);
        }
        m_ReadyForNextWave = true;
    }

    private void SpawnWave()
    {
        foreach (AIZombie zombie in m_LivingZombies)
        {
            zombie.gameObject.SetActive(true);
        }
        m_ReadyForNextWave = false;
    }
}
