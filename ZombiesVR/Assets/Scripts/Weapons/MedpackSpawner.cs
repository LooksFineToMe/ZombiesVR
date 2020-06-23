using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedpackSpawner : MonoBehaviour
{
    public GameObject[] medpack;
    public Transform[] spawnLocations;
    int maxSpawnLocations;
    int randomNumber;
    public int startGameMags = 3;
    // Start is called before the first frame update
    void Start()
    {
        maxSpawnLocations = spawnLocations.Length;
        for (int i = 0; i < startGameMags; i++)
        {
            SpawnMedpack();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    [ContextMenu("SpawnAmmo")]
    public void SpawnMedpack()
    {
        randomNumber = Random.Range(0, maxSpawnLocations);
        int randommagazine = Random.Range(0, medpack.Length);
        Instantiate(medpack[randommagazine], spawnLocations[randomNumber].transform.position, Quaternion.identity);
    }
}
