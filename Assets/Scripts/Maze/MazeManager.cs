using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MazeManager : MonoBehaviour
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
    public bool hasSelectedAnswer = false;


    private GameManager gamemanager;
    private Pregunta pregunta;
    [SerializeField]
    GameObject[] triggers;
    [SerializeField]
    GameObject gema;
    [SerializeField]
    GameObject calavera;

    public TimeManager tm;
   

    List<Vector3> positions = new List<Vector3>
    {
        new Vector3(-0.9f, -8.7f, 0),
        new Vector3(7.4f, 0.6f, 0),
        new Vector3(-10.8f, 0.3f, 0),
        new Vector3(-2.3f, -7.4f, 0)
    };

    void Start()
    {
        gamemanager = FindObjectOfType<GameManager>();
        tm = gamemanager.GetTimeManager();
        tm.SetTimeRemaining(tiempo);
        pregunta = gamemanager.GetCurrentQuestion();
        preguntaTexto.text = pregunta.textoPregunta;
        for (int i = 0; i < 4; i++)
        {
            Respuesta res = pregunta.respuestas[i];
            respuestasTexto[i].text = res.textoRespuesta;
            MoveCamara mc = triggers[i].GetComponent<MoveCamara>();
            mc.idRespuesta = res.idRespuesta;
            if (res.esCorrecta == 1)
            {
                mc.esCorrecta = true;
                Instantiate(gema, positions[i], Quaternion.identity);
            }
            else
            {
                Instantiate(calavera, positions[i], Quaternion.identity);
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
            }
            else
            {
                gamemanager.OnWrongAnswer(-1);
            }
        }
    }
}
