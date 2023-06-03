using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleCrash : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed = 4f;
    public bool esCorrecta = false;
    public int idRespuesta = -1;
    private TopDownCar_Manager controller;
    private GameManager gameManager;
    private AudioSource aus;
    [SerializeField] private AudioClip correct;
    [SerializeField] private AudioClip incorrect;

    void Start()
    {
        controller = FindObjectOfType<TopDownCar_Manager>();
        gameManager = FindObjectOfType<GameManager>();
        aus = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.gameOver && !controller.reachedEnd)
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
                aus.clip = correct;
                aus.Play();
                StartCoroutine(FinishGame(true, idRespuesta));
            }
            else
            {
                aus.clip = incorrect;
                aus.Play();
                controller.reachedEnd = true;
                collision.gameObject.GetComponent<AudioSource>().Stop();
                StartCoroutine(FinishGame(true, idRespuesta));
            }
        }
    }

    IEnumerator FinishGame(bool win, int idRespuesta)
    {
        yield return new WaitForSeconds(2);
        if (win)
        {
            gameManager.OnCorrectAnswer(idRespuesta);

        }
        else
        {
            gameManager.OnWrongAnswer(idRespuesta);
        }
    }

}