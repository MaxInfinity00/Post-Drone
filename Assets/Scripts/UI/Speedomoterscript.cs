using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Speedomoterscript : MonoBehaviour
{

    public Sprite[] speedomoterImage;
    public Image speedomoter;
    public DroneMovement droneMovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        speedomoter.sprite = speedomoterImage[droneMovement.WhatGearAmI()];



    }
}
