using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class PowerManager : MonoBehaviour
{

    static private int cantidadPoderes = 3;

    private Power[] poderesExamen;
    private Power[] poderesAlumno = new Power[cantidadPoderes];
    private bool[] active = new bool[cantidadPoderes];
    
    public bool[] GetActive()
    {
        return active;
    }

    public Power[] GetPoderesAlumno()
    {
        return poderesAlumno;
    }

    public int GetPoderesLength()
    {
        return poderesAlumno.Length;
    }

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        StartCoroutine(GetPoderesAl());
    }

    IEnumerator GetPoderesAl()
    {
        int idAlumno = PlayerPrefs.GetInt("IdAlumno");
        string URL = PlayerPrefs.GetString("ApiPrefix") + "alumno/" + idAlumno + "/poderes";

        var req = UnityWebRequest.Get(URL);

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        handleResultAlumno(req);

        req.Dispose();
    }

    void handleResultAlumno(UnityWebRequest req)
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
            Power[] poderes = JsonHelper.getJsonArray<Power>(req.downloadHandler.text);
            foreach (Power p in poderes) {
                poderesAlumno[p.idPoder - 1] = p;
            }
            StartCoroutine(GetPoderesEx());

        }
    }

    IEnumerator GetPoderesEx()
    {
        int idExamen = PlayerPrefs.GetInt("IdExamen");
        string URL = PlayerPrefs.GetString("ApiPrefix") + "examenpoder/" + idExamen;

        var req = UnityWebRequest.Get(URL);

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        handleResultExamen(req);

        req.Dispose();
    }

    void handleResultExamen(UnityWebRequest req)
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
            poderesExamen = JsonHelper.getJsonArray<Power>(req.downloadHandler.text);
            foreach(Power p in poderesExamen)
            {
                active[p.idPoder-1] = true;
            }
        }
    }

}
