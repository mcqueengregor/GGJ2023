using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject[] CamPos;
    public int Index = 0;
    public float CamSpeed = 2f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position == CamPos[Index].transform.position)
        {
            Index++;
        }

        if (Index <= CamPos.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, CamPos[Index].transform.position, (CamSpeed * Time.deltaTime));
        }
    }
}
