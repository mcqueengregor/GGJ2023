using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //public GameObject[] Enemies;
    [Header("Enemy Prefab Setting")]
    public GameObject Enemy;
    //public GameObject Spawner;

    [Header("Spawn Area Limits")]
    public Transform AreaSpawn1;
    public Transform AreaSpawn2;

    [Header("Max Enemies on Screen")]
    public float MaxEnemScreen = 1;
    private float EnemOnScreen = 0f;
    [Header("Amount Able to Spawn at the Same Time")]
    public float MaxSameSpawnLimit = 1;
    private float SameSpawn = 1f;


    private float SpawnTimer = 2;

    [Header("Spawn Timers")]
    public float MinTime = 1;
    public float MaxTime = 5;
    

    private bool Spawned = false;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnEnem", 2.0f);
    }

    void SpawnEnem()
    {
        SameSpawn = Random.Range(1, MaxSameSpawnLimit);
        for (int i = 0; i < SameSpawn; i++)
        {
            var SpawnPosition = new Vector3(Random.Range(AreaSpawn1.position.x, AreaSpawn2.position.x), Random.Range(AreaSpawn1.position.y, AreaSpawn2.position.y), Random.Range(AreaSpawn1.position.z, AreaSpawn2.position.z));
            GameObject Clone = Instantiate(Enemy, SpawnPosition, Quaternion.identity);
        }
        SpawnTimer = Random.Range(MinTime, MaxTime);
        Invoke("SpawnEnem", SpawnTimer);
    }
}
