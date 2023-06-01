using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AnswerSubmitManager : MonoBehaviour
{
    public int IdPregunta {get; set; }
    public int IdRespuesta { get; set; }

    public void SendAnswer()
    {
        StartCoroutine("PostAnswer");
    }

    IEnumerator PostAnswer()
    {
        string URL = PlayerPrefs.GetString("ApiPrefix") + "alumno/pregunta";

        AnswerData answerData = new AnswerData();
        answerData.IdAlumno = PlayerPrefs.GetInt("IdAlumno");
        answerData.IdPregunta = IdPregunta;
        answerData.IdRespuesta = IdRespuesta;

        string json = JsonUtility.ToJson(answerData);

        var req = new UnityWebRequest(URL, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        handleResult(req);

        req.Dispose();
    }


    void handleResult(UnityWebRequest req)
    {
        if (req.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(req.error);
        }
        else if (req.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log("Respuesta enviada correctamente");
        }
    }
}

