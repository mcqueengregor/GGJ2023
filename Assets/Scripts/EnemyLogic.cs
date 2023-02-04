using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public SpriteRenderer EnemBase;
    public Sprite[] Sprites;




    // Start is called before the first frame update
    void Start()
    {
        EnemBase = GetComponent<SpriteRenderer>();
        EnemBase.sprite = Sprites[Random.Range(0, Sprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
