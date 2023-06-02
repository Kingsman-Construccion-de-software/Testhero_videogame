using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class UserLogin : MonoBehaviour
{
    [SerializeField] TMP_InputField correo;
    [SerializeField] TMP_InputField password;
    [SerializeField] TMP_Text errorCorreo;
    [SerializeField] TMP_Text errorPassword;
    [SerializeField] TMP_Text errorLogin;

    public void Start()
    {
        FindObjectOfType<BackgroundMusic>().PlayInicio();
    }


    public void validateAlumno()
    {

        //datos para pruebas
        //TODO: Remover antes de deployment
        if (correo.text == "alumno@prueba.com" && password.text == "alumnoprueba")
        {
            PlayerPrefs.SetInt("IdAlumno", -1);
            PlayerPrefs.Save();
            SceneController sce = FindObjectOfType<SceneController>();
            sce.CambiaEscena("IngresaCodigo");
            return;
        }

        if (correo.text.Length > 0  && password.text.Length > 0)
        {
            StartCoroutine("PostAlumno");
        } else
        {
            errorLogin.text = "Debes llenar todos los datos";
        } 
    }

    public void ShowFeedbackCorreo()
    {
        if(correo.text.Length == 0)
        {
            errorCorreo.text = "Debes ingresar un correo";
        }
    }

    public void ShowFeedbackPassword()
    {
        if (password.text.Length == 0)
        {
            errorPassword.text = "Debes ingresar una contraseña";
        }
    }

    public void HideFeedbackCorreo()
    {
        errorCorreo.text = "";   
    }

    public void HideFeedbackPassword()
    {
        errorPassword.text = ""; 
    }

    IEnumerator PostAlumno()
    {
        string URL = PlayerPrefs.GetString("ApiPrefix") + "login/alumno";
        
        AlumnoData alumnoData = new AlumnoData();
        alumnoData.correo = correo.text;
        alumnoData.password = password.text;

        string json = JsonUtility.ToJson(alumnoData);

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
        else
        {
            LoginResponse response = new LoginResponse();
            response = JsonUtility.FromJson<LoginResponse>(req.downloadHandler.text);
            if (response.message != "Login exitoso")
            {
                errorLogin.text = response.message;
            } else
            {
                PlayerPrefs.SetInt("IdAlumno", response.id);
                PlayerPrefs.SetInt("IdGrupo", response.idGrupo);
                SceneController sc = FindObjectOfType<SceneController>();
                sc.CambiaEscena("IngresaCodigo");
            }
        }
    }

}
