using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("AI And Player Specific")]
    [SerializeField] List<AIZombie> m_Zombies;
    [HideInInspector] public List<AIZombie> m_LivingZombies;
    [SerializeField] public List<GameObject> m_SpawnLocations;
    [SerializeField] public List<GameObject> m_Players;
    [SerializeField] public ComboManager s_ComboManager;

    [Header("Waves Specific")]
    [Tooltip("How long between waves and when the next wave begins")]
    [SerializeField] float m_TimeOffset = 10f;
    [SerializeField] public int m_CurrentWave;
    [Tooltip("The number of tracks we're using. A track will randomly picked when a wave starts.")]
    [SerializeField] int m_AmountOfTracks = 2;
    [HideInInspector] public int m_ChosenTrack;
    private float m_NextWave;
    private bool m_Break;

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

        //m_TimeOffset = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        //if next wave is ready, spawn them enemies
        if (m_ReadyForNextWave && m_NextWave <= Time.time)
        {
            SpawnWave();
            m_CurrentValueOfWave = 0;
            m_Break = false;
        }

        //if all zombies are dead create the next wave to spawn
        if (m_LivingZombies.Count <= 0)
        {
            m_WaveSpawnValue += m_IncrementValues;
            CreateWave();
            m_Break = true; //just for debugging but could use for something else
            if(m_CurrentWave != 0)
                s_ComboManager.BreakSong();
        }
    }

    //this works for the first wave but not for everything else
    private void CreateWave()
    {
        m_NextWave = Time.time + m_TimeOffset;
        //this could be buggy
        while (m_CurrentValueOfWave < m_WaveSpawnValue)
        {
            AIZombie zombie = Instantiate(m_Zombies[Random.Range(0, m_Zombies.Count - 1)]);
            zombie.transform.parent = gameObject.transform;
            zombie.transform.position = m_SpawnLocations[Random.Range(0, m_SpawnLocations.Count - 1)].transform.position;
            zombie.SetWaveManager(this, s_ComboManager);
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
        m_CurrentWave += 1;
        PickTrack();
    }

    private void PickTrack()
    {
        m_ChosenTrack = Random.Range(0, m_AmountOfTracks);
        print("Chosen Track: " + m_ChosenTrack.ToString());
    }
}
