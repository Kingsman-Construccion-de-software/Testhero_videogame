using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    [SerializeField] TMP_Text respuestasCorrectas;
    [SerializeField] TMP_Text puntosTexto;


    GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        int totalPoints = gameManager.GetTotalPoints();
        int correct = gameManager.GetRespuestasCorrectas();
        int total = gameManager.GetTotalPreguntas();

        puntosTexto.text = totalPoints.ToString();
        respuestasCorrectas.text = $"SACASTE {correct}/{total}";

        gameManager.SaveExam();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            gameManager.LoadRanking();
        }
    }
}
