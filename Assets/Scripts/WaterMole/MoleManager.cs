using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoleManager : MonoBehaviour
{
    [SerializeField] GameObject BubbleArea;

    [SerializeField]
    private TMP_Text preguntaTexto;

    [SerializeField]
    private TMP_Text tiempoText;

    [SerializeField]
    private float tiempoPregunta = 30f;

    [SerializeField]
    private List<GameObject> answerSprites; // List of answer sprites

    public bool gameOver = false;
    List<Vector3> positions = new List<Vector3>
    {
        new Vector3(-16f, 4.3f, 0),
        new Vector3(-0.2f, -1.2f, 0),
        new Vector3(16.5f, -6.2f, 0),
        new Vector3(0f, -0.5f, 0)
    };

    private TimeManager tm;

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
            TMP_Text answerText = answerSprites[i].GetComponentInChildren<TMP_Text>();
            answerText.text = res.textoRespuesta;
        }

        // cargar el tiempo
        tm = gamemanager.GetTimeManager();
        tm.SetTimeRemaining(tiempoPregunta);
    }

    private bool playerInsideCollider = false; // Flag to track if the player is inside the collider
    private bool canSpawnSprite = true; // Flag to track if a sprite can be spawned

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true; // Set flag to true when the player enters the collider
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false; // Set flag to false when the player exits the collider
        }
    }

    void Update()
    {
        if (!gameOver)
        {
            if (!tm.IsDone())
            {
                int time = Mathf.CeilToInt(tm.GetTimeRemaining());
                string timeT = "0:" + time.ToString();
                tiempoText.text = timeT;
                if (time <= 5)
                {
                    tiempoText.color = new Color(253 / 255f, 77 / 255f, 77 / 255f);
                }
            }
            else
            {
                gamemanager.OnWrongAnswer(-1);
            }
        }
    }

    public void OnCorrectAnswer(int idRespuesta)
    {
        ClearAnswers();
        gameOver = true;
        tm.Stop();
        StartCoroutine(FinishGame(true, idRespuesta));
    }

    public void OnWrongAnswer(int idRespuesta)
    {
        ClearAnswers();
        gameOver = true;
        tm.Stop();
        StartCoroutine(FinishGame(false, idRespuesta));
    }

    void ClearAnswers()
    {
        foreach (GameObject answerSprite in answerSprites)
        {
            answerSprite.SetActive(false);
        }
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