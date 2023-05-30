using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;



public class ExamConnectionManager : MonoBehaviour
{
    public int IdAlumno { get; set; }
    public int IdExamen { get; set; }
    public int Calificacion { get; set; }
    public int Puntaje { get; set; }

    public void SubmitExam()
    {
        StartCoroutine("PostExam");
    }


    IEnumerator PostExam()
    {
        string URL = PlayerPrefs.GetString("ApiPrefix") + "alumno/examen";

        ExamData examData = new ExamData();
        examData.IdAlumno = PlayerPrefs.GetInt("IdAlumno");
        examData.IdExamen = IdExamen;
        examData.Calificacion = Calificacion;
        examData.Puntaje = Puntaje;

        string json = JsonUtility.ToJson(examData);

        var req = new UnityWebRequest(URL, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        handleResultPost(req);

        req.Dispose();
    }


    void handleResultPost(UnityWebRequest req)
    {
        if (req.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(req.error);
        } else if(req.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log("Examen enviado correctamente");
        }
    }
}

