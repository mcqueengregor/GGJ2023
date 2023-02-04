using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public SpriteRenderer EnemBase;
    public Sprite[] Sprites;

    public float Speed = 2;

    public Vector3 EndTarg;
    public GameObject Target1;
    public GameObject Target2;


    // Start is called before the first frame update
    void Start()
    {
        EnemBase = GetComponent<SpriteRenderer>();
        EnemBase.sprite = Sprites[Random.Range(0, Sprites.Length)];
        
        Target1 = GameObject.FindGameObjectWithTag("EnemyTarget1");
        Target2 = GameObject.FindGameObjectWithTag("EnemyTarget2");
        EndTarg = new Vector3(Random.Range(Target1.transform.position.x, Target2.transform.position.x), Random.Range(Target1.transform.position.y, Target2.transform.position.y), Random.Range(Target1.transform.position.z, Target2.transform.position.z));
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(EndTarg);
        transform.position = Vector3.MoveTowards(transform.position, EndTarg, (Speed * Time.deltaTime));
    }
}
