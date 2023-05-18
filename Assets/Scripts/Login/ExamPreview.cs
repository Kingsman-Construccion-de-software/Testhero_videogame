using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class Etiqueta
{
    public int idEtiqueta;
    public string nombre;
}



public class ExamPreview : MonoBehaviour
{

    [SerializeField] List<Button> etiquetas;


    private void Awake()
    {
        PlayerPrefs.SetInt("IdExamen", 19);
        StartCoroutine("GetEtiquetas");
    }


    IEnumerator GetEtiquetas()
    {
        int idExamen = PlayerPrefs.GetInt("IdExamen");
        string URL = PlayerPrefs.GetString("ApiPrefix") + "etiqueta/examen/" + idExamen;

        var req = UnityWebRequest.Get(URL);

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        HandleResult(req);

        req.Dispose();
    }

    void HandleResult(UnityWebRequest req)
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
            Etiqueta[] etiquetas = JsonHelper.getJsonArray<Etiqueta>(req.downloadHandler.text);
            RenderTags(etiquetas);
        }
    }

    void RenderTags(Etiqueta[] tags)
    {
        int numTags = tags.Length;
        numTags = Math.Min(numTags, etiquetas.Count);
        for(int i = 0; i<numTags; i++)
        {
            etiquetas[i].gameObject.SetActive(true);
            etiquetas[i].GetComponentInChildren<TMP_Text>().text = tags[i].nombre;
        }
    }


    public void LoadGame()
    {
        SceneController sc = FindObjectOfType<SceneController>();
        sc.CambiaEscena("MarioGame");
    }
}
