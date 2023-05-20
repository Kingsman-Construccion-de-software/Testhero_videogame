using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{

    [SerializeField] TMP_Text textoCorrecto;
    [SerializeField] TMP_Text textoPuntos;



    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        int points = gameManager.GetAddedPoints();
        if (points > 0)
        {
            textoCorrecto.text = "¡Correcto!";
            textoPuntos.text = "+" + points.ToString();
        } else
        {
            textoCorrecto.text = "¡Incorrecto!";
            textoPuntos.text = points.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            gameManager.AdvanceGame();
        }
    }
}
