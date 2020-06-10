using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Mag : MonoBehaviour
{
    public GameObject[] magToSpawn;
    public Transform[] spawnLocations;
    int maxSpawnLocations;
    int randomNumber;
    public int startGameMags = 10;
    // Start is called before the first frame update
    void Start()
    {
        maxSpawnLocations = spawnLocations.Length;
        for (int i = 0; i < startGameMags; i++)
        {
            SpawnAmmo();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    [ContextMenu("SpawnAmmo")]
    public void SpawnAmmo()
    {
        randomNumber = Random.Range(0, maxSpawnLocations);
        int randommagazine = Random.Range(0, magToSpawn.Length);
        Instantiate(magToSpawn[randommagazine], spawnLocations[randomNumber].transform.position, Quaternion.identity);
    }
}
