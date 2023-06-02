using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpaceShipGameController : MonoBehaviour
{
    [SerializeField] TMP_Text QuestionText;

    GameManager _GameManager;
    Pregunta Question;
    public int ans;

    private TimeManager tm;
    [SerializeField] TMP_Text timeText;
    [SerializeField] float tiempoPregunta = 15f;

    // Answer option texts
    [SerializeField] TMP_Text AnswerOptionText1;
    [SerializeField] TMP_Text AnswerOptionText2;
    [SerializeField] TMP_Text AnswerOptionText3;
    [SerializeField] TMP_Text AnswerOptionText4;

    void Awake() {
        _GameManager = FindObjectOfType<GameManager>();
        Question = _GameManager.GetCurrentQuestion();
        QuestionText.text = Question.textoPregunta;


        for(int i=0;i<4;i++) {
            Respuesta answer = Question.respuestas[i];
            if(answer.esCorrecta == 1) {
                ans = i;
            }
        }
    }

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            if(i == 0) {
                AnswerOptionText1.text = Question.respuestas[i].textoRespuesta;
            } else if(i == 1) {
                AnswerOptionText2.text = Question.respuestas[i].textoRespuesta;
            } else if(i == 2) {
                AnswerOptionText3.text = Question.respuestas[i].textoRespuesta;
            } else if(i == 3) {
                AnswerOptionText4.text = Question.respuestas[i].textoRespuesta;
            }
        }

        tm = _GameManager.GetTimeManager();
        tm.SetTimeRemaining(tiempoPregunta);
    }

    // Update is called once per frame
    void Update()
    {
        if (!tm.IsDone())
        {
            int time = Mathf.CeilToInt(tm.GetTimeRemaining());
            string timeT = "0:" + time.ToString();
            timeText.text = timeT;
            if (time <= 5)
            {
                timeText.color = new Color(253/255f, 77/255f, 77/255f);
            }
        }
        else if(tm.IsDone()) {
            OnWrongAnswer(-1);
        }
    }

    public void OnCorrectAnswer(int idRespuesta)
    {
        tm.Stop();
        StartCoroutine(FinishGame(true, idRespuesta));
    }

    public void OnWrongAnswer(int idRespuesta)
    {
        tm.Stop();
        StartCoroutine(FinishGame(false, idRespuesta));
    }

    IEnumerator FinishGame(bool win, int idRespuesta)
    {
        yield return new WaitForSeconds(3);
        if (win)
        {
            _GameManager.OnCorrectAnswer(idRespuesta);
        } else
        {
            _GameManager.OnWrongAnswer(idRespuesta);
        }
    }
}
