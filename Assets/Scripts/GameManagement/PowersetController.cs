using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PowersetController : MonoBehaviour
{

    [SerializeField] List<TMP_Text> cantidades;
    [SerializeField] List<Image> powers;
    bool[] unavailable;

    private GameManager gameManager;
    private bool gameOver = false;

    private int totalPoderes;

    public void SetGameOver(bool over)
    {
        gameOver = over;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        totalPoderes = gameManager.GetTotalPowers();
        unavailable = new bool[totalPoderes];

        for (int i = 0; i < totalPoderes; i++)
        {
            int cantidad = gameManager.GetPowerAmount(i);
            cantidades[i].text = cantidad.ToString();
            if (!gameManager.GetPowerActive(i) || cantidad == 0)
            {
                powers[i].color = new Color(0.5f, 0.5f, 0.5f, 1);
            }

            if (SceneManager.GetActiveScene().name.Equals("Feedback"))
            {
                unavailable[1] = true;
                unavailable[2] = true;
                powers[1].color = new Color(0.5f, 0.5f, 0.5f, 1);
                powers[2].color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
            else
            {
                unavailable[0] = true;
                powers[0].color = new Color(0.5f, 0.5f, 0.5f, 1);
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                RepeatQuestion();   
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                IncreaseTime();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
               
            }
        }
    }

    void IncreaseTime()
    {
        if (!unavailable[1] && gameManager.GetPowerAmount(1) > 0 && gameManager.GetPowerActive(1))
        {
            //add time
            TimeManager tm = gameManager.GetTimeManager();
            tm.AddTime(10f);

            //decrease amount
            gameManager.DecreasePowerAmount(1);
            unavailable[1] = true;


            //update amount in GM and UI
            int cantidad = gameManager.GetPowerAmount(1);
            cantidades[1].text = cantidad.ToString();
            powers[1].color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }

    void RepeatQuestion()
    {
        if (!unavailable[0] && gameManager.GetPowerAmount(0) > 0 && gameManager.GetPowerActive(0) && gameManager.GetNumberIncorrect() > 0)
        {
            //repeat question
            gameManager.RetryQuestion();

            //decrease amount
            gameManager.DecreasePowerAmount(0);
            unavailable[0] = true;

            //update amount in GM and UI
            int cantidad = gameManager.GetPowerAmount(0);
            cantidades[0].text = cantidad.ToString();
            powers[0].color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }

}
