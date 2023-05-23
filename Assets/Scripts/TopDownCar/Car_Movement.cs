using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Movement : MonoBehaviour
{
    public new Transform transform;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    private TopDownCar_Manager controller;
    private GameManager gamemanager;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<TopDownCar_Manager>(); // para algo que tiene otra cosa pero está en la escena
        // GetComponent para algo que está en el script
        gamemanager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.gameOver)
        {
            Movement();
            Clamp();
        }
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(0, 0, 43),
                rotationSpeed * Time.deltaTime
            );
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(0, 0, 137),
                rotationSpeed * Time.deltaTime
            );
        }
        if (transform.rotation.z != 90)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(0, 0, 90),
                10f * Time.deltaTime
            );
        }
        // Falta movimeinto en posición Y
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }
    }

    void Clamp()
    {
        // Chocar con lados (limites posición 6.38)
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -6.38f, 6.38f);
        pos.y = Mathf.Clamp(pos.y, -3.58f, 3.58f);
        transform.position = pos;
    }
}
