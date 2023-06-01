using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class QuestionManager : MonoBehaviour
{

    [SerializeField] private Pregunta[] preguntasExamen;
    public Pregunta GetPregunta(int idPregunta)
    {
        return preguntasExamen[idPregunta];
    }

    public int GetPreguntasSize()
    {
        return preguntasExamen.Length;
    }

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        //Para el caso de prueba
        //TODO: eliminar para deployment
        if (PlayerPrefs.GetInt("IdExamen") == -1)
        {
            Respuesta[] respuestas1 = {
                new Respuesta(-10, "10", 0, -1) ,
                new Respuesta(-11, "40", 0, -1),
                new Respuesta(-12, "35", 0, -1),
                new Respuesta(-13, "50", 1, -1)
            };

            Respuesta[] respuestas2 = {
                new Respuesta(-20, "Rojo, amarillo y azul", 1, -2) ,
                new Respuesta(-21, "Rojo, verde, violeta", 0, -2),
                new Respuesta(-21, "Azul, negro, blanco", 0, -2),
                new Respuesta(-21, "Amarillo, verde, azul", 0, -2)
            };

            Respuesta[] respuestas3 = {
                new Respuesta(-30, "1", 0, -3) ,
                new Respuesta(-31, "4", 0, -3),
                new Respuesta(-31, "7", 1, -3),
                new Respuesta(-31, "10", 0, -3)
            };

            Respuesta[] respuestas4 = {
                new Respuesta(-40, "1000g", 1, -4) ,
                new Respuesta(-41, "450g", 0, -4),
                new Respuesta(-41, "500g", 0, -4),
                new Respuesta(-41, "1g", 0, -4)
            };

            Respuesta[] respuestas5 = {
                new Respuesta(-20, "Orden Natural del Universo", 0, -5) ,
                new Respuesta(-21, "Organización Nacional Unida", 0, -5),
                new Respuesta(-21, "Organización de las Naciones Unidas", 1, -5),
                new Respuesta(-21, "Orden, Nación y Unidad", 0, -5)
            };

            Pregunta[] preguntas = {
                new Pregunta(-1, -1, "¿Cuántas estrellas tiene la bandera de EUA?", respuestas1),
                new Pregunta(-2, -1, "¿Cuáles son los 3 colores primarios?", respuestas2),
                new Pregunta(-3, -1, "¿Cuántas maravillas hay en el mundo?", respuestas3),
                new Pregunta(-4, -1, "¿Cuántos gramos equivale a un kilogramo?", respuestas4),
                new Pregunta(-5, -1, "¿Qué significan las siglas ONU?", respuestas5)
            };
            

            preguntasExamen = preguntas;
            return;
        }


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
