using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformerManager : MonoBehaviour
{

    [SerializeField] GameObject chest;
    [SerializeField] TMP_Text preguntaText;

    [SerializeField] TMP_Text timeText;
    [SerializeField] float tiempoPregunta = 15f;

    public bool gameOver = false;

    List<Vector3> positions = new List<Vector3>
    {
        new Vector3(6f, -9f, 0),
        new Vector3(-4f, -3.9f, 0),
        new Vector3(4.2f, 3f, 0),
        new Vector3(-15.2f, 5f, 0)
    };

    private TimeManager tm;

    GameManager gameManager;
    Pregunta pregunta;

    List<GameObject> chestGameObjects = new List<GameObject>();

    private AudioSource aus;
    [SerializeField] private AudioClip correct;
    [SerializeField] private AudioClip incorrect;

    private PowersetController powc;
    Color timeColor;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        pregunta = gameManager.GetCurrentQuestion();
        preguntaText.text = pregunta.textoPregunta;
        aus = GetComponent<AudioSource>();
        powc = FindObjectOfType<PowersetController>();
        timeColor = timeText.color;

        //crear cofres en la escena
        for (int i = 0; i < 4; i++)
        {
            Respuesta resp = pregunta.respuestas[i];
            GameObject chestGo = Instantiate(chest, positions[i], chest.transform.rotation);
            chestGo.GetComponentInChildren<TMP_Text>().text = resp.textoRespuesta;

            Chest chestScript = chestGo.GetComponent<Chest>();
            chestScript.IdRespuesta = resp.idRespuesta;
            if (resp.esCorrecta == 1)
            {
                chestScript.ToggleCorrectAnswer();
            }
            chestGameObjects.Add(chestGo);
        }
        //cargar el tiempo
        tm = gameManager.GetTimeManager();
        tm.SetTimeRemaining(tiempoPregunta);
    }

    private void Update()
    {
        if (!gameOver)
        {
            if (!tm.IsDone())
            {
                int time = Mathf.CeilToInt(tm.GetTimeRemaining());
                string timeT = "0:" + time.ToString();
                timeText.text = timeT;
                if (time <= 5)
                {
                    timeText.color = new Color(253/255f, 77/255f, 77/255f);
                } else
                {
                    timeText.color = timeColor;
                }
            }
            else
            {
                OnWrongAnswer(-1);
            }
        }
    }


    public void OnCorrectAnswer(int idRespuesta)
    {
        ClearAnswers();
        gameOver = true;
        tm.Stop();
        aus.clip = correct;
        aus.Play();
        powc.SetGameOver(true);

        StartCoroutine(FinishGame(true, idRespuesta));
    }

    public void OnWrongAnswer(int idRespuesta)
    {
        ClearAnswers();
        gameOver = true;
        tm.Stop();
        aus.clip = incorrect;
        aus.Play();
        powc.SetGameOver(true);

        StartCoroutine(FinishGame(false, idRespuesta));
    }

    void ClearAnswers()
    {
        foreach(GameObject go in chestGameObjects)
        {
            go.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
            go.GetComponentInChildren<Chest>().Open();
        }
    }

    IEnumerator FinishGame(bool win, int idRespuesta)
    {
        yield return new WaitForSeconds(3);
        if (win)
        {
            gameManager.OnCorrectAnswer(idRespuesta);
        } else
        {
            gameManager.OnWrongAnswer(idRespuesta);
        }
    }



}
