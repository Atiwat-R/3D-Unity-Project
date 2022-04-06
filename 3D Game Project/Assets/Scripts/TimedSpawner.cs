using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawner : MonoBehaviour
{

    public GameObject spawnee; // Specify what Prefab to spawn
    public bool stopSpawning = false;

    [SerializeField] float spawnTime; // How long before we start the spawning
    [SerializeField] float spawnDelay; // Delay between each
    [SerializeField] int maximumSpawnee; // Maximum spawnee that can exist at once


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
    }

    // Update is called once per frame
    void Update() {
        // Debug.Log(GameObject.FindGameObjectsWithTag("Enemy").Length);

        // If enemy has decreased below limit, turn it on again. 
        if (stopSpawning == true && GameObject.FindGameObjectsWithTag("Enemy").Length <= maximumSpawnee) {
            InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
        }
    }

    void SpawnObject()
    {
        Instantiate(spawnee, transform.position, transform.rotation);

        // If there's too many spawnee, turn off spawner.
        if (GameObject.FindGameObjectsWithTag("Enemy").Length > maximumSpawnee) {
            stopSpawning = true;
            CancelInvoke("SpawnObject");
        }
    }
}
