using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    [SerializeField]
    float tiempo = 15f;

    [SerializeField]
    private List<TMP_Text> respuestasTexto;

    [SerializeField]
    private List<Image> arrows;

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
        new Vector3(-1.5f, 7.7f, 0),
        new Vector3(7.6f, -0.4f, 0),
        new Vector3(-10.4f, 0.6f, 0),
        new Vector3(-0.5f, -7.5f, 0),
    };


    Color timeColor;
    private bool markedIncorrect = false;
    [SerializeField]
    private GameObject mark;


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
                else
                {
                    tiempoTexto.color = timeColor;
                }
            }
            else
            {
                gamemanager.OnWrongAnswer(-1);
            }
        }

        if (gamemanager.ShouldMarkIncorrect() && !markedIncorrect)
        {
            MarkIncorrect();
        }
    }


    private void MarkIncorrect()
    {
        markedIncorrect = true;
        List<Image> incorrectArrows = new List<Image>();

        for(int i = 0; i<4; i++)
        {
            Image arrow = arrows[i];
            Respuesta res = pregunta.respuestas[i];
            if(res.esCorrecta != 1)
            {
                incorrectArrows.Add(arrow);
            }
        }

        System.Random rnd = new System.Random();
        int r = rnd.Next(3);
        SpriteRenderer sr = mark.GetComponent<SpriteRenderer>();
        incorrectArrows[r].sprite = sr.sprite;
        incorrectArrows[r].color = sr.color;


    }

}
