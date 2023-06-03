using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Explosion : MonoBehaviour
{

    SpaceShipGameController controller;
    AudioSource aus;
    private void Start()
    {
        controller = FindObjectOfType<SpaceShipGameController>();
        aus = GetComponent<AudioSource>();
        
        if (controller.gameOver)
        {
            aus.Stop();
        }
    }
}
