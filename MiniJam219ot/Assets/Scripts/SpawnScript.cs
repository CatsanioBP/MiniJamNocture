using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class SpawnScript : MonoBehaviour
{
    [Header("References to Objects")]
    
    [SerializeField]GameObject player;
    [SerializeField]GameObject batEnemy;
    [SerializeField]GameObject spawnLocationsHolder;
    [SerializeField]List<Transform> spawnPositions;

    [Header("Timers")]
    float spawnTimer;
    // TIME TO SPAWN 
    [SerializeField]float spawnTimerLength;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //MOVES THE SPAWN LOCATIONS AROUND THE PLAYER
        spawnLocationsHolder.transform.position = player.transform.position;
        spawnTimer += Time.deltaTime; //MAKES THE ENEMY SPAWN AFTER A DECIDED TIME
        if(spawnTimer >= spawnTimerLength)
        {
            spawnAnEnemy();
            spawnTimer = 0f;
        }
    }

    void spawnAnEnemy()
    {
        // SPAWNS AN ENEMY AT A CHOSEN SPOT
        Transform chosenSpawn;
        chosenSpawn = spawnPositions[Random.Range(0, spawnPositions.Count)];
        GameObject spawnedEnemy = Instantiate(batEnemy, new Vector3(chosenSpawn.position.x + Random.Range(-5f, 5f), chosenSpawn.position.y + Random.Range(-5f, 5f), 0 ), Quaternion.identity);
        spawnedEnemy.TryGetComponent<BaseEnemy>(out BaseEnemy baseEnemyScript); // GETS THE BaseEnemy SCRIPT INSIDE THE ENEMY AND ALLOWS US TO SEND CHANGES TO IT
        baseEnemyScript.target = player;
    }
}
