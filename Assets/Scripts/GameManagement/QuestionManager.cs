using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Pregunta
{
    public int idPregunta;
    public int idExamen;
    public string textoPregunta;
    public Respuesta[] respuestas;
}


[System.Serializable]
public class Respuesta
{
    public int idRespuesta;
    public string textoRespuesta;
    public int esCorrecta;
    public int idPregunta;
}

//pregunta/examen/idExamen
//respuesta/pregunta/idPregunta

public class QuestionManager : MonoBehaviour
{

    [SerializeField] private Pregunta[] preguntasExamen;
    Pregunta getPreguntas(int idPregunta)
    {
        return preguntasExamen[idPregunta];
    }

    int preguntasSize()
    {
        return preguntasExamen.Length;
    }

    public void Awake()
    {
        PlayerPrefs.SetInt("IdExamen", 19);
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        StartCoroutine("GetPreguntas");
    }

    IEnumerator GetPreguntas()
    {
        int idExamen = PlayerPrefs.GetInt("IdExamen");
        string URL = PlayerPrefs.GetString("ApiPrefix") + "pregunta/examen/" + idExamen;

        var req = UnityWebRequest.Get(URL);

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        handleResultPreguntas(req);

        req.Dispose();
    }

    void handleResultPreguntas(UnityWebRequest req)
    {
        if (req.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(req.error);
        }
        else if (req.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(req.error);
        }
        else
        {
            preguntasExamen = JsonHelper.getJsonArray<Pregunta>(req.downloadHandler.text);
            foreach(Pregunta pregunta in preguntasExamen)
            {
                StartCoroutine(GetRespuestas(pregunta));
            }

        }
    }

    IEnumerator GetRespuestas(Pregunta pregunta)
    {
        string URL = PlayerPrefs.GetString("ApiPrefix") + "respuesta/pregunta/" + pregunta.idPregunta;

        var req = UnityWebRequest.Get(URL);

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        handleResultRespuestas(req, pregunta);

        req.Dispose();
    }

    void handleResultRespuestas(UnityWebRequest req, Pregunta pregunta)
    {
        if (req.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(req.error);
        }
        else if (req.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Respuesta[] respuestas = JsonHelper.getJsonArray<Respuesta>(req.downloadHandler.text);
            pregunta.respuestas = respuestas;
        }
    }

}
