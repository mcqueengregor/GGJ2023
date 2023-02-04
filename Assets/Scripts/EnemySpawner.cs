using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //public GameObject[] Enemies;
    public GameObject Enemy;
    public GameObject Spawner;
    
    
    public Transform AreaSpawn1;
    public Transform AreaSpawn2;


    public float MaxEnemScreen = 1;
    public float MaxSameSpawnLimit = 1;
    public float SpawnTimer = 2;
    public float MaxTime = 5;
    public float MinTime = 1;

    public bool Spawned = false;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnEnem", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void SpawnEnem()
    {
        Spawned = true;
        print("done");
        var SpawnPosition = new Vector3(Random.Range(AreaSpawn1.position.x, AreaSpawn2.position.x), Random.Range(AreaSpawn1.position.y, AreaSpawn2.position.y), Random.Range(AreaSpawn1.position.z, AreaSpawn2.position.z));
        GameObject Clone = Instantiate(Enemy, SpawnPosition, Quaternion.identity);
        SpawnTimer = Random.Range(MinTime, MaxTime);
        Invoke("SpawnEnem", SpawnTimer);
    }
}
