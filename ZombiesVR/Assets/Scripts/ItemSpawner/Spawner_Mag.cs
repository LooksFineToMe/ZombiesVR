using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Mag : MonoBehaviour
{
    public GameObject magToSpawn;
    public Transform[] spawnLocations;
    int maxSpawnLocations;
    int randomNumber;
    // Start is called before the first frame update
    void Start()
    {
        maxSpawnLocations = spawnLocations.Length;
    }

    // Update is called once per frame
    void Update()
    {

    }
    [ContextMenu("SpawnAmmo")]
    public void SpawnAmmo()
    {
        randomNumber = Random.Range(0, maxSpawnLocations);
        Instantiate(magToSpawn, spawnLocations[randomNumber].transform.position, Quaternion.identity);
    }
}
