using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerDistance : MonoBehaviour
{
    [SerializeField] GameObject m_Player;
    [SerializeField] float m_ToggleDistance = 5f;
    [SerializeField] WaveManager m_Spawner;

    private bool added;
    private bool removed;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_Spawner.m_SpawnLocations.Remove(this.gameObject);
            gameObject.GetComponent<MeshRenderer>().enabled = false; //for testing
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_Spawner.m_SpawnLocations.Add(this.gameObject);
            gameObject.GetComponent<MeshRenderer>().enabled = true; //for testing
        }
    }
}
