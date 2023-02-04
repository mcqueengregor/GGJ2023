using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    //how fast the progress bar fills
    public float FillSpeed = 0.2f;
    //What value the progress bar will stop at
    public float targetProgress = 0;
    //slider for value changes
    private Slider slider;
    
    private void Awake()
    {
        //get instance of slider in scene
        slider = gameObject.GetComponent<Slider>();
    }

    void Start()
    {
        //testing for increment
        IncrementProgress(0.95f);
    }

    // Update is called once per frame
    void Update()
    {
        if(slider.value < targetProgress)
        {
            //fill the progress bar over time until it reaches the target progress
            slider.value += FillSpeed * Time.deltaTime;
        }
    }

    public void IncrementProgress(float newProgress)
    {
        //set target progress to player's position in scene
        //set the progress bar fill to increase overtime 
        targetProgress = slider.value + newProgress;
    }
}
