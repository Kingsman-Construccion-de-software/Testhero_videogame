using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road_Animation : MonoBehaviour
{
    public Renderer meshRenderer;
    public float speed = 0.5f;
    private TopDownCar_Manager controller;


    private void Start()
    {
        controller = FindObjectOfType<TopDownCar_Manager>();
    }

    void Update()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
    }

}
