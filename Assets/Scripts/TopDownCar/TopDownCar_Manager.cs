using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopDownCar_Manager : MonoBehaviour
{
    [SerializeField]
    private List<TMP_Text> respuestasTexto;

    [SerializeField]
    private TMP_Text preguntaTexto;

    [SerializeField]
    private TMP_Text tiempoTexto;
    public bool gameOver = false;
    private GameManager gamemanager;
    private Pregunta pregunta;

    void Start()
    {
        gamemanager = FindObjectOfType<GameManager>();
        pregunta = gamemanager.GetCurrentQuestion();
        preguntaTexto.text = pregunta.textoPregunta;
        for (int i = 0; i < 4; i++)
        {
            Respuesta res = pregunta.respuestas[i];
            respuestasTexto[i].text = res.textoRespuesta;
        }
    }

    // Update is called once per frame
    void Update() { }
}
