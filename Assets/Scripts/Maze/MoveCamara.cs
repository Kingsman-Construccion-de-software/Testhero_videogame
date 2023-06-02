using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamara : MonoBehaviour
{
    [SerializeField]
    float verticalOffset = 8f;
    [SerializeField]
    float horizontalOffset = 10f;
    public int idRespuesta = -1;
    private GameManager gamemanager;
    public bool esCorrecta = false;
    private MazeManager controller;
    // Start is called before the first frame update
    void Start()
    {
        gamemanager = FindObjectOfType<GameManager>();
        controller = FindObjectOfType<MazeManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!controller.hasSelectedAnswer)
        {
            if (gameObject.name == "TriggerRight")
            {
                Camera.main.transform.Translate(horizontalOffset, 0, 0);
            }
            else if (gameObject.name == "TriggerUp")
            {
                Camera.main.transform.Translate(0, verticalOffset, 0);
            }
            else if (gameObject.name == "TriggerLeft")
            {
                Camera.main.transform.Translate(-horizontalOffset, 0, 0);
            }
            else
            {
                Camera.main.transform.Translate(0, -verticalOffset, 0);
            }

            //stop time and select answer
            controller.tm.Stop();
            controller.hasSelectedAnswer = true;
            //hide ui objects
            GameObject[] UIObjs = GameObject.FindGameObjectsWithTag("UI");
            foreach(GameObject go in UIObjs)
            {
                go.SetActive(false);
            }
            //start coroutine to finish game
            StartCoroutine(FinishGame(esCorrecta, idRespuesta));
        }
        
    }

    IEnumerator FinishGame(bool win, int idRespuesta)
    {
        yield return new WaitForSeconds(2);
        controller.gameOver = true;
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
