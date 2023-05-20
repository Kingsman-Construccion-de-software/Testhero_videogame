using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int currentGame;

    private SceneController sc;
    private QuestionManager qm;
    private TimeManager tm;

    private List<Minigame> allMinigames;
    private List<Minigame> currMinigames;

    private Pregunta currentQuestion;
    private List<int> respuestas;

    [SerializeField] int wonPoints = 2000;
    [SerializeField] int bonusPoints = 100;
    [SerializeField] int lostPoints = 500;
    [SerializeField] int totalPoints;
    int addedPoints;
    int respuestasCorrectas = 0;

    private ExamConnectionManager examConnection;
    private AnswerSubmitManager asm;

    public Pregunta GetCurrentQuestion()
    {
        return currentQuestion;
    }

    public TimeManager GetTimeManager()
    {
        return tm;
    }

    public int GetAddedPoints()
    {
        return addedPoints;
    }

    public int GetTotalPoints()
    {
        return totalPoints;
    }

    public int GetRespuestasCorrectas()
    {
        return respuestasCorrectas;
    }

    public int GetTotalPreguntas()
    {
        return qm.GetPreguntasSize();
    }

    private void Awake()
    {
        PlayerPrefs.SetString("ApiPrefix", "https://localhost:44423/api/");
        PlayerPrefs.Save();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGame = 0;
        totalPoints = 0;
        InitializeMinigames();
        sc = FindObjectOfType<SceneController>();
        DontDestroyOnLoad(this.gameObject);
    }

    void InitializeMinigames()
    {
        allMinigames = new List<Minigame>
        {
            Minigame.MarioGame,
            Minigame.Maze,
            Minigame.WaterMole,
            Minigame.Spaceship,
            Minigame.TopDownCar
        };
    }

    public void PrepareGame()
    {
        qm = FindObjectOfType<QuestionManager>();
        if(qm.GetPreguntasSize() > 0)
        {
            currMinigames = new List<Minigame>();
            respuestas = new List<int>();

            for (int i = 0; i < qm.GetPreguntasSize(); i++)
            {
                //agregar los minijuegos
                //TODO: Hacerlo en orden aleatorio
                currMinigames.Add(allMinigames[0]);
            }

            currentQuestion = qm.GetPregunta(0);
            tm = gameObject.AddComponent<TimeManager>();
            LoadNextGame();
        } else
        {
            Debug.Log("No hay preguntas");
        } 
    }

    public void OnCorrectAnswer(int idRespuesta)
    {
        sc.CambiaEscena("Feedback");

        int time = Mathf.CeilToInt(tm.GetTimeRemaining());

        addedPoints = wonPoints + time * bonusPoints;
        totalPoints += addedPoints;

        respuestasCorrectas++;
        respuestas.Add(idRespuesta);
    }


    public void OnWrongAnswer(int idRespuesta)
    {
        sc.CambiaEscena("Feedback");

        addedPoints = -lostPoints;
        totalPoints += addedPoints;

        respuestas.Add(idRespuesta);
    }

    public void AdvanceGame()
    {
        currentGame++;
        if(currentGame < qm.GetPreguntasSize())
        {
            //cargar siguiente minijuego
            currentQuestion = qm.GetPregunta(currentGame);
            LoadNextGame();
        }
        else
        {
            //llevar a la pantalla de resultados
            LoadSummary();
        }
    }

    public void SaveExam()
    {
        if (respuestas.Count == GetTotalPreguntas())
        {
            examConnection = FindObjectOfType<ExamConnectionManager>();
            asm = FindObjectOfType<AnswerSubmitManager>();
            examConnection.IdAlumno = PlayerPrefs.GetInt("IdAlumno");
            examConnection.IdExamen = PlayerPrefs.GetInt("IdExamen");
            double calificacion = (double)respuestasCorrectas / (double)qm.GetPreguntasSize() * 100;
            examConnection.Calificacion = (int) calificacion;
            examConnection.Puntaje = totalPoints;
            examConnection.SubmitExam();

            for (int i = 0; i < qm.GetPreguntasSize(); i++)
            {
                asm.IdPregunta = qm.GetPregunta(i).idPregunta;
                asm.IdRespuesta = respuestas[i];
                asm.SendAnswer();
            }
        } else
        {
            Debug.Log("No se ha completado el examen");
        }
    }

    void LoadNextGame()
    {
        OpenMinigame(currMinigames[currentGame]);
    }

   void LoadSummary()
    {
        sc.CambiaEscena("Results");
    }

    public void LoadRanking()
    {
        sc.CambiaEscena("Ranking");
    }


    void OpenMinigame(Minigame minigame)
    {
        sc.CambiaEscena(minigame.ToString());
    }

}