using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Option : MonoBehaviour
{
    public int idRespuesta = 0;
    private GameManager gamemanager;
    public bool isCorrect = false;

    void Start()
    {
        gamemanager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Opción");
        StartCoroutine(FinishGame(isCorrect, idRespuesta));
    }

    IEnumerator FinishGame(bool win, int idRespuesta)
    {
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
