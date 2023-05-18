using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int currentGame;

    private SceneController sc;
    private QuestionManager qm;
    private TimeManager tm;

    private List<Minigame> allMinigames;
    private List<Minigame> currMinigames;

    [SerializeField] private Pregunta currentQuestion;

    [SerializeField] int totalPoints;
    int addedPoints;
    int respuestasCorrectas = 0;
    
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
            Minigame.NightDriver
        };
    }

    public void PrepareGame()
    {
        qm = FindObjectOfType<QuestionManager>();
        if(qm.GetPreguntasSize() > 0)
        {
            currMinigames = new List<Minigame>();
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

    public void OnCorrectAnswer(int basePoints)
    {
        sc.CambiaEscena("Feedback");
        addedPoints = basePoints;
        respuestasCorrectas++;
        totalPoints += addedPoints;
    }


    public void OnWrongAnswer(int basePoints)
    {
        sc.CambiaEscena("Feedback");
        addedPoints = -basePoints;
        totalPoints += addedPoints;
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

    void LoadNextGame()
    {
        OpenMinigame(currMinigames[currentGame]);
    }

   void LoadSummary()
    {
        sc.CambiaEscena("Results");
    }


    void OpenMinigame(Minigame minigame)
    {
        sc.CambiaEscena(minigame.ToString());
    }

}