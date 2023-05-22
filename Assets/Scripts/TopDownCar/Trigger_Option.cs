using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Option : MonoBehaviour
{
    public int idRespuesta = -1;
    private GameManager gamemanager;
    public bool esCorrecta = false;
    private TopDownCar_Manager controller;

    void Start()
    {
        gamemanager = FindObjectOfType<GameManager>();
        controller = FindObjectOfType<TopDownCar_Manager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!controller.hasSelectedAnswer)
        {
            Debug.Log("Opción");
            controller.tm.Stop();
            StartCoroutine(FinishGame(esCorrecta, idRespuesta));
        }
    }

    IEnumerator FinishGame(bool win, int idRespuesta)
    {
        controller.hasSelectedAnswer = true; 
        yield return new WaitForSeconds(2);
        controller.gameOver = true;
        yield return new WaitForSeconds(3);
        if (win)
        {
            gamemanager.OnCorrectAnswer(idRespuesta);
        }
        else
        {
            gamemanager.OnWrongAnswer(idRespuesta);
        }
    }
}