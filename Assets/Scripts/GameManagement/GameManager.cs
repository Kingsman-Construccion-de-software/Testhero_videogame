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

    private List<Minigame> allMinigames;
    private List<Minigame> currMinigames;

    private void Awake()
    {
        PlayerPrefs.SetString("ApiPrefix", "https://localhost:44423/api/");
        PlayerPrefs.Save();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGame = 0;
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
        currMinigames = new List<Minigame>();
        for(int i = 0; i < 5; i++)
        {
            currMinigames.Add(allMinigames[0]);
        }
        OpenScene(currMinigames[0]);
    }

    void OpenScene(Minigame minigame)
    {
        sc.CambiaEscena(minigame.ToString());
    }

    void Update()
    {
        string name = SceneManager.GetActiveScene().name;

    }
}