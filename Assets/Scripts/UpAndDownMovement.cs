using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDownMovement : MonoBehaviour
{
    public GameObject[] Pos;
    public int Index = 0;
    public float ObjSpeed = 2f;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((Index + 1) > Pos.Length)
        {
            Index = 0;
        }
        print(Pos.Length);


        print(Index);


        if (gameObject.transform.position == Pos[Index].transform.position)
        {
            Index++;
        }



        if (Index <= Pos.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, Pos[Index].transform.position, (ObjSpeed * Time.deltaTime));
        }


    }
}
