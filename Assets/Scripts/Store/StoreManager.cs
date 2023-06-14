using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreManager : MonoBehaviour
{

    [SerializeField]
    private TMP_Text tickets;
    private SceneController sc;

    private int available = 0;
    private int spent = 0;

    private int[] powers = new int[3] { 0, 0, 0 };
    private int[] indices = { 1, 2, 0 };
    private int[] costs = { 150, 250, 400 };

    [SerializeField]
    private List<TMP_Text> cantidadTextos;

    [SerializeField] List<Image> powersImages;
    private GameManager gameManager;
    private int[] bases = new int[3] { 0, 0, 0 };

    [SerializeField] AudioClip incSound;
    [SerializeField] AudioClip decSound;
    private AudioSource aus;


    public void IncreaseAmount(int i)
    {
        if (gameManager.GetPowerActive(indices[i]) && available - costs[i] >= 0)
        {
            powers[i]++;
            spent += costs[i];
            cantidadTextos[i].text = (bases[i] + powers[i]).ToString();
            available -= costs[i];
            tickets.text = available.ToString();
            powersImages[i].color = new Color(1f, 1f, 1f, 1);

            aus.clip = incSound;
            aus.Play();

        }
    }

    public void DecreaseAmount(int i)
    {
        if (gameManager.GetPowerActive(indices[i]) &&  powers[i] > 0)
        {
            powers[i]--;
            spent -= costs[i];
            cantidadTextos[i].text = (bases[i] + powers[i]).ToString();
            available += costs[i];
            tickets.text = available.ToString();

            aus.clip = decSound;
            aus.Play();

            if (bases[i] + powers[i] == 0)
            {
                powersImages[i].color = new Color(0.5f, 0.5f, 0.5f, 1);
            }

        }
    }

    public void Exit()
    {
        StartCoroutine(PutPowers(0));
    }

    // Start is called before the first frame update
    void Start()
    {
        sc = FindObjectOfType<SceneController>();
        StartCoroutine(GetTickets());
        aus = GetComponent<AudioSource>();

        gameManager = FindObjectOfType<GameManager>();

        BackgroundMusic bm = FindObjectOfType<BackgroundMusic>();
        bm.PlayTienda();


        for (int i = 0; i < 3; i++)
        {
            int idx = indices[i];
            int cantidad = gameManager.GetPowerAmount(idx);
            cantidadTextos[i].text = cantidad.ToString();
            bases[i] = cantidad;
            if (!gameManager.GetPowerActive(idx) || cantidad == 0)
            {
                powersImages[i].color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
        }
    }
    
    IEnumerator GetTickets()
    {
        int idAlumno = PlayerPrefs.GetInt("IdAlumno");
        string URL = PlayerPrefs.GetString("ApiPrefix") + "alumno/" + idAlumno + "/tickets";

        var req = UnityWebRequest.Get(URL);

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        handleResultGet(req);

        req.Dispose();
    }

    void handleResultGet(UnityWebRequest req)
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
            Ticket t = JsonUtility.FromJson<Ticket>(req.downloadHandler.text);
            available = t.tickets;
            tickets.text = available.ToString();
        }
    }

    IEnumerator PutTickets()
    {
        int idAlumno = PlayerPrefs.GetInt("IdAlumno");
        string URL = PlayerPrefs.GetString("ApiPrefix") + "alumno/" + idAlumno + "/tickets";


        Ticket ticket = new Ticket();
        ticket.tickets = -spent;

        string json = JsonUtility.ToJson(ticket);

        var req = new UnityWebRequest(URL, "PUT");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        handleResultPutTickets(req);

        req.Dispose();
    }

    void handleResultPutTickets(UnityWebRequest req)
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
            FindObjectOfType<GameManager>().InitializePoderes();
            FindObjectOfType<GameManager>().LoadNextGame();
        }
    }


    IEnumerator PutPowers(int id)
    {
        int idAlumno = PlayerPrefs.GetInt("IdAlumno");
        string URL = PlayerPrefs.GetString("ApiPrefix") + "alumno/" + idAlumno + "/poderes";


        Power p = new Power();
        p.idPoder = indices[id] + 1;
        p.cantidad = powers[id];

        string json = JsonUtility.ToJson(p);

        var req = new UnityWebRequest(URL, "PUT");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        handleResultPutPowers(req, id);

        req.Dispose();
    }

    void handleResultPutPowers(UnityWebRequest req, int id)
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
            if(id == 2)
            {
                StartCoroutine(PutTickets());
            } else
            {
                StartCoroutine(PutPowers(id+1));
            }
        }
    }


}