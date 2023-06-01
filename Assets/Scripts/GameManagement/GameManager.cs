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
            //Minigame.WaterMole,
            //Minigame.Spaceship,
            Minigame.TopDownCar
        };
        currMinigames = new List<Minigame>();
    }

    public void PrepareGame()
    {
        qm = FindObjectOfType<QuestionManager>();
        if(qm.GetPreguntasSize() > 0)
        {
            respuestas = new List<int>();

            currMinigames = ChooseGames(qm.GetPreguntasSize());

            /*
            for (int i = 0; i < qm.GetPreguntasSize(); i++)
            {
                //currMinigames.Add(allMinigames[4]);
            }
            */
            
            currentQuestion = qm.GetPregunta(0);
            tm = gameObject.AddComponent<TimeManager>();
            LoadNextGame();
        } else
        {
            Debug.Log("No hay preguntas");
        } 
    }

    //seleccionar la lista de juegos evitando repeticiones por 2 turnos
    public List<Minigame> ChooseGames(int n)
    {
        List<Minigame> lastMinigames = new List<Minigame>(3);
        List<Minigame> listMinigames = new List<Minigame>();

        System.Random rnd = new System.Random();
        int randomNumber;
        int totalN = allMinigames.Count;

        for(int i = 0; i<n; i++)
        {
            do
            {
                randomNumber = rnd.Next(0, totalN);
            } while(lastMinigames.Contains(allMinigames[randomNumber]));
            if (lastMinigames.Count == 2)
            {
                lastMinigames.RemoveAt(0);
            }
            lastMinigames.Add(allMinigames[randomNumber]);
            listMinigames.Add(allMinigames[randomNumber]);
        }

        return listMinigames;
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