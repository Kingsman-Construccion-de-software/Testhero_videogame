using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Option : MonoBehaviour
{
    private GameManager gamemanager;
    private TopDownCar_Manager controller;
    private TimeManager tm;
    private PowersetController powc;


    void Start()
    {
        gamemanager = FindObjectOfType<GameManager>();
        controller = FindObjectOfType<TopDownCar_Manager>();
        powc = FindObjectOfType<PowersetController>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!controller.gameOver)
            {
                controller.StopTime();
                controller.HideUI();
                controller.gameOver = true;
                powc.SetGameOver(true);

            }
        }
   
    }
}
