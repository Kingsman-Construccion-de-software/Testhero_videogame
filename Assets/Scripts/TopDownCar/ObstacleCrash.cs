using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCrash : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed = 4f;
    public bool esCorrecta = false;
    public int idRespuesta = -1;
    private TopDownCar_Manager controller;
    private GameManager gameManager;


    void Start()
    {
        controller = FindObjectOfType<TopDownCar_Manager>();
        gameManager = FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (controller.gameOver)
        {            
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (esCorrecta)
            {
                gameManager.OnCorrectAnswer(idRespuesta);
            }
            else
            {
                gameManager.OnWrongAnswer(idRespuesta);
            }
        }
    }

}