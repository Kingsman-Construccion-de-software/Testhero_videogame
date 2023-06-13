using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public bool reachedEnd = false;
    // public bool hasSelectedAnswer = false; 

    private GameManager gamemanager;
    private Pregunta pregunta;

    [SerializeField]
    GameObject[] triggers;

    [SerializeField]
    GameObject finishLine;

    [SerializeField]
    GameObject obstacle;

    public TimeManager tm;

    [SerializeField]
    private Canvas canvas;

    List<Vector3> positions = new List<Vector3>
    {
        new Vector3(-6f, 6f, 0),
        new Vector3(-2f, 6f, 0),
        new Vector3(2f, 6f, 0),
        new Vector3(6f, 6f, 0),
    };

    Color timeColor;
    private bool markedIncorrect = false;

    [SerializeField]
    private List<GameObject> crosses;


    void Start()
    {
        gamemanager = FindObjectOfType<GameManager>();
        tm = gamemanager.GetTimeManager();
        tm.SetTimeRemaining(tiempo);
        pregunta = gamemanager.GetCurrentQuestion();
        preguntaTexto.text = pregunta.textoPregunta;
        timeColor = tiempoTexto.color;

        for (int i = 0; i < 4; i++)
        {
            Respuesta res = pregunta.respuestas[i];
            respuestasTexto[i].text = res.textoRespuesta;
            if (res.esCorrecta == 1)
            {
                GameObject go = Instantiate(finishLine, positions[i], Quaternion.identity);
                go.GetComponent<ObstacleCrash>().idRespuesta = res.idRespuesta;
            }
            else
            {
                GameObject go =  Instantiate(obstacle, positions[i], Quaternion.identity);
                go.GetComponent<ObstacleCrash>().idRespuesta = res.idRespuesta;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (!tm.IsDone())
            {
                int time = Mathf.CeilToInt(tm.GetTimeRemaining());
                string timeT = "0:" + time.ToString();
                tiempoTexto.text = timeT;
                if (time <= 5)
                {
                    tiempoTexto.color = new Color(253 / 255f, 77 / 255f, 77 / 255f);
                }
                else
                {
                    tiempoTexto.color = timeColor;
                }
            }
            else
            {
                gamemanager.OnWrongAnswer(-1);
            }

            if (gamemanager.ShouldMarkIncorrect() && !markedIncorrect)
            {
                MarkIncorrect();
            }
        }

    }

    private void MarkIncorrect()
    {
        markedIncorrect = true;
        List<GameObject> incorrectPaths = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            GameObject cross = crosses[i];
            Respuesta res = pregunta.respuestas[i];
            if (res.esCorrecta != 1)
            {
                incorrectPaths.Add(cross);
            }
        }

        System.Random rnd = new System.Random();
        int r = rnd.Next(3);
        incorrectPaths[r].SetActive(true);


    }

    public void StopTime()
    {
        tm.Stop();
    }

    public void HideUI()
    {
        canvas.gameObject.SetActive(false);
    }
}
