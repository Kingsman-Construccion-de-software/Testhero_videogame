using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

public class TopDownCar_Manager : MonoBehaviour
{
    [SerializeField]
    float tiempo = 15f;

    [SerializeField]
    private List<TMP_Text> respuestasTexto;

    [SerializeField]
    private TMP_Text preguntaTexto;

    [SerializeField]
    private TMP_Text tiempoTexto;
    public bool gameOver = false;

    private GameManager gamemanager;
    private Pregunta pregunta;

    [SerializeField]
    GameObject[] triggers;

    [SerializeField]
    GameObject[] finishLine;

    [SerializeField]
    GameObject[] obstacle;

    public TimeManager tm;

    List<Vector3> positions = new List<Vector3>
    {
        new Vector3(-5.4f, 6f, 0),
        new Vector3(-1.8f, 6f, 0),
        new Vector3(1.8f, 6f, 0),
        new Vector3(5.4f, 6f, 0),
    };

    void Start()
    {
        gamemanager = FindObjectOfType<GameManager>();
        tm = gamemanager.GetTimeManager();
        pregunta = gamemanager.GetCurrentQuestion();
        preguntaTexto.text = pregunta.textoPregunta;
        for (int i = 0; i < 4; i++)
        {
            Respuesta res = pregunta.respuestas[i];
            respuestasTexto[i].text = res.textoRespuesta;
            Trigger_Option to = triggers[i].GetComponent<Trigger_Option>();
            to.isCorrect = res.idRespuesta; // revisar
            if (res.isCorrect == 1)
            {
                to.isCorrect = true;
                Instantiate(finishLine, positions[i], Quaternion.identity);
            }
            else
            {
                Instantiate(obstacle, positions[i], Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update() { }
}
