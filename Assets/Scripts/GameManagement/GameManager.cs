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
    private PowerManager pm;
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

    [SerializeField] private Power[] poderesAlumno;
    [SerializeField] private bool[] active;

    private ExamConnectionManager examConnection;
    private AnswerSubmitManager asm;

    private BackgroundMusic bm;

    //variables para el manejo del poder de repetir pregunta
    private List<int> incorrectQuestions = new List<int>();
    private bool retrying = false;

    //variable para el manejo del poder de marcar incorrecto
    private bool markIncorrect = false;

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

    public bool GetPowerActive(int i)
    {
        return active[i];
    }

    public int GetTotalPowers()
    {
        return poderesAlumno.Length;
    }

    public int GetPowerAmount(int i)
    {
        return poderesAlumno[i].cantidad;
    }

    public void DecreasePowerAmount(int i)
    {
        poderesAlumno[i].cantidad--;
        int id = poderesAlumno[i].idPoder;
        pm.DecreaseAmount(id);
    }

    public void IncreasePowerAmount(int i)
    {
        poderesAlumno[i].cantidad++;
    }

    public int GetNumberIncorrect() {
        return incorrectQuestions.Count;
    }

    public void MarkIncorrect()
    {
        markIncorrect = true;
    }

    public bool ShouldMarkIncorrect()
    {
        return markIncorrect;
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
        bm = FindObjectOfType<BackgroundMusic>();
        sc = FindObjectOfType<SceneController>();
        DontDestroyOnLoad(this.gameObject);
    }

    void InitializePoderes()
    {
        poderesAlumno = pm.GetPoderesAlumno();
        active = pm.GetActive();
    }

    void InitializeMinigames()
    {
        allMinigames = new List<Minigame>
        {
            Minigame.MarioGame,
            Minigame.Maze,
            Minigame.TopDownCar,
            Minigame.WaterMole,
            Minigame.Spaceship,
        };
        currMinigames = new List<Minigame>();
    }

    public void PrepareGame()
    {
        pm = FindObjectOfType<PowerManager>();
        InitializePoderes();

        qm = FindObjectOfType<QuestionManager>();

        if (qm.GetPreguntasSize() > 0)
        {
            respuestas = new List<int>();

            //currMinigames = ChooseGames(qm.GetPreguntasSize());

            
            for (int i = 0; i < qm.GetPreguntasSize(); i++)
            {
                currMinigames.Add(allMinigames[2]);
            }
            
            
            
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

        int start = 0;

        //se maneja el caso de reintento
        //se agregan los últimos elementos a la lista de anteriores para evitar repetirlos
        //se mantienen los elementos anteriores de la lista y se generan de nuevo los elementos restantes
        if (retrying)
        {
            int numEls = Math.Min(3, currentGame + 1);
            
            for(int i = 0; i< numEls; i++)
            {
                lastMinigames.Add(currMinigames[currentGame - numEls + 1]);
            }

            start = currentGame;
            for(int i = 0; i<currentGame; i++)
            {
                listMinigames.Add(currMinigames[i]);
            }
        } 

        //se generan los elementos de manera aleatoria
        for (int i = start; i < n; i++)
        {
            do
            {
                randomNumber = rnd.Next(0, totalN);
            } while (lastMinigames.Contains(allMinigames[randomNumber]));
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

        markIncorrect = false;

        respuestasCorrectas++;
        respuestas.Add(idRespuesta);
    }


    public void OnWrongAnswer(int idRespuesta)
    {
        sc.CambiaEscena("Feedback");

        addedPoints = -lostPoints;
        totalPoints += addedPoints;

        markIncorrect = false;

        incorrectQuestions.Add(currentGame);

        respuestas.Add(idRespuesta);
    }

    public void RetryQuestion()
    {
        retrying = true;
    }

    public void AdvanceGame()
    {

        if (retrying) {
            System.Random rnd = new System.Random();
            int index = rnd.Next(incorrectQuestions.Count);
            currentQuestion = qm.GetPregunta(index);
            currMinigames = ChooseGames(qm.GetPreguntasSize());
            LoadNextGame();
            retrying = false;
        } else
        {
            currentGame++;
            if (currentGame < qm.GetPreguntasSize())
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
        bm.PlayMinigame((int) minigame);
    }

}