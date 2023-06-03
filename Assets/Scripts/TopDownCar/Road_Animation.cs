using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road_Animation : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 initialPosition;
    private BoxCollider2D collider;
    private float boundy;
    private TopDownCar_Manager manager;

    private void Start()
    {
        initialPosition = transform.position;
        collider = GetComponent<BoxCollider2D>();
        boundy = initialPosition.y - collider.size.y / 2;
        manager = FindObjectOfType<TopDownCar_Manager>();
    }

    void Update()
    {
        if (!manager.reachedEnd)
        {

            transform.position += Time.deltaTime * speed * Vector3.down;
            if (transform.position.y < boundy)
            {
                transform.position = initialPosition;
            }
        }
    }

}
