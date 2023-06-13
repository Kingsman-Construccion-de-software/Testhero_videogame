using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowersetController : MonoBehaviour
{

    [SerializeField] List<TMP_Text> cantidades;
    [SerializeField] List<Image> powers;
    bool[] used;

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
        used = new bool[totalPoderes];

        for (int i = 0; i < totalPoderes; i++)
        {
            int cantidad = gameManager.GetPowerAmount(i);
            cantidades[i].text = cantidad.ToString();
            if (!gameManager.GetPowerActive(i) || cantidad == 0)
            {
                powers[i].color = new Color(0.5f, 0.5f, 0.5f, 1);
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
        if (!used[1] && gameManager.GetPowerAmount(1) > 0 && gameManager.GetPowerActive(1))
        {
            //add time
            TimeManager tm = gameManager.GetTimeManager();
            tm.AddTime(10f);

            //decrease amount
            gameManager.DecreasePowerAmount(1);
            used[1] = true;


            //update amount in GM and UI
            int cantidad = gameManager.GetPowerAmount(1);
            cantidades[1].text = cantidad.ToString();
            powers[1].color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }

}
