using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private static string nextScene;
    [SerializeField] private int currentGame;

    private List<string> games = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        currentGame = 0;
        DontDestroyOnLoad(this.gameObject);
        games.Add("MarioGame");
        games.Add("Maze");
        games.Add("NightDriver");
        games.Add("spaceship");
        games.Add("WaterMole");
    }

    void Update()
    {
        string name = SceneManager.GetActiveScene().name;

        if (Input.GetKey(KeyCode.X))
        {
            if(name == "Feedback")
            {
                currentGame++;
                if (currentGame < games.Count)
                {
                    SceneManager.LoadScene(games[currentGame]);
                }
                else
                {
                    SceneManager.LoadScene("Results");
                }
            } else if (games.Contains(name))
            {
                SceneManager.LoadScene("Feedback");
            } else if(name == "Results")
            {
                SceneManager.LoadScene("Ranking");
            } else if(name == "Ranking"){
                 SceneManager.LoadScene("Final");
            }
        }

    }
}