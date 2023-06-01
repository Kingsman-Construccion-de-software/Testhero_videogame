using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class CodeForm : MonoBehaviour
{
    [SerializeField] TMP_InputField codigoInput;
    [SerializeField] TMP_Text codigoError;
    [SerializeField] TMP_Text submitError;
    Examen examen;

    public void validateCodigo()
    {
        if (codigoInput.text.Length == 8)
        {
            StartCoroutine("PostCodigo");
        }
        else
        {
            submitError.text = "Debes ingresar el código";
        }
    }

    public void ShowFeedbackCodigo()
    {
        if (codigoInput.text.Length == 0)
        {
            codigoError.text = "Debes ingresar el código";
        }
        else if (codigoInput.text.Length != 8)
        {
            codigoError.text = "El código debe tener 8 caracteres";
        }
    }

    public void HideFeedbackCodigo()
    {
        codigoError.text = "";
    }

 
    IEnumerator PostCodigo()
    {
        string c = codigoInput.text;
        string URL = PlayerPrefs.GetString("ApiPrefix") + "examen/codigo/" + c;

        var req = UnityWebRequest.Get(URL);

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        handleResult(req);

        req.Dispose();
    }

    void handleResult(UnityWebRequest req)
    {
        if (req.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(req.error);
            submitError.text = "No se encontró el examen. Intenta de nuevo.";
        } else if(req.result == UnityWebRequest.Result.ConnectionError)
        {
            submitError.text = "Se produjo un error interno. Intenta de nuevo.";
        }
        else
        {
            examen = new Examen();
            examen = JsonUtility.FromJson<Examen>(req.downloadHandler.text);
            if(PlayerPrefs.GetInt("IdGrupo") == examen.idGrupo)
            {
                PlayerPrefs.SetInt("IdExamen", examen.idExamen);
                PlayerPrefs.SetString("TituloExamen", examen.nombre);
                PlayerPrefs.Save();
                StartCoroutine("CheckCompleted");
            } else
            {
                submitError.text = "No eres miembro de este grupo. Consúltalo con tu profesor.";
            }
            
        }
    }

    //se valida si el usuario ya ha completado el examen

    IEnumerator CheckCompleted()
    {
        int IdAlumno = PlayerPrefs.GetInt("IdAlumno");
        int IdExamen = PlayerPrefs.GetInt("IdExamen");

        string URL = PlayerPrefs.GetString("ApiPrefix") + "alumno/examen/" + IdExamen + "/" + IdAlumno;

        var req = UnityWebRequest.Get(URL);

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        handleResultCompleted(req);

        req.Dispose();
    }

    void handleResultCompleted(UnityWebRequest req)
    {
        if (req.result == UnityWebRequest.Result.ProtocolError)
        {
            //el alumno no ha completado el examen
            validarFecha(examen);
        }
        else if (req.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error de conexión");
        }
        else
        {
            Score score = new Score();
            score = JsonUtility.FromJson<Score>(req.downloadHandler.text);
            SceneController sc = FindObjectOfType<SceneController>();
            sc.CambiaEscena("ExamCompleted");
        }
    }

    void validarFecha(Examen examen)
    {
        DateTime now = DateTime.Now;
        DateTime inicio = DateTime.ParseExact(examen.fechaInicio, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
        DateTime fin = DateTime.ParseExact(examen.fechaFin, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);

        if (DateTime.Compare(inicio, now) > 0)
        {
            submitError.text = "Este examen todavía no se encuentra disponible.";
        }
        else if (DateTime.Compare(fin, DateTime.Now) < 0)
        {
            submitError.text = "Este examen ya no se encuentra disponible";
        }
        else
        {
            SceneController sc = FindObjectOfType<SceneController>();
            sc.CambiaEscena("ExamPreview");
        }
    }
}
