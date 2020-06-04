using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JerryCanSpawner : MonoBehaviour
{
    public GameObject[] m_JerryCans;
    public Transform[] m_1stSpawnLocations;
    public Transform[] m_2ndSpawnLocations;
    public Transform[] m_3rdSpawnLocations;
    public Transform[] m_4thSpawnLocations;
    public Transform[] m_5thSpawnLocations;

    private void Start()
    {
        //Spawns the 1st jerrycan at location
        int randomLocationIndex = Random.Range(0, m_1stSpawnLocations.Length);
        m_JerryCans[0].transform.position = m_1stSpawnLocations[randomLocationIndex].transform.position;
        //2ND JERRYCAN
        randomLocationIndex = Random.Range(0, m_2ndSpawnLocations.Length);
        m_JerryCans[1].transform.position = m_2ndSpawnLocations[randomLocationIndex].transform.position;
        //3RD JERRYCAN SPAWN
        randomLocationIndex = Random.Range(0, m_3rdSpawnLocations.Length);
        m_JerryCans[2].transform.position = m_3rdSpawnLocations[randomLocationIndex].transform.position;
        //4TH JERRYCAN
        randomLocationIndex = Random.Range(0, m_4thSpawnLocations.Length);
        m_JerryCans[3].transform.position = m_3rdSpawnLocations[randomLocationIndex].transform.position;
        //5TH JERRYCAN
        randomLocationIndex = Random.Range(0, m_5thSpawnLocations.Length);
        m_JerryCans[4].transform.position = m_3rdSpawnLocations[randomLocationIndex].transform.position;

    }
}
