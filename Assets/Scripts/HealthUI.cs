using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider Bar;
    public GameObject Player;
    public PlayerHealth HealthScript;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        HealthScript = Player.GetComponent<PlayerHealth>();
    }

    public void MAXHealth(int Max)
    {
        Bar.maxValue = Max;
        Bar.value = Max;
    }


    public void SliderValue(int Health)
    {
        Bar.value = Health;
    }

}
