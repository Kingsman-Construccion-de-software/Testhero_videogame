using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class TicketsManager : MonoBehaviour
{

    private int totalAmount;
    private int currentAmount = 0;
    private GameManager gm;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text continueText;
    SceneController sc;


    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        totalAmount = gm.GetTotalPoints();
        totalAmount /= 100;
        sc = FindObjectOfType<SceneController>();
    }

    void Update()
    {
        if (totalAmount > 0 && currentAmount < totalAmount)
        {
            currentAmount += 5;
            scoreText.text = currentAmount.ToString();
        }


        if (currentAmount >= totalAmount)
        {
            continueText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(PostTickets());
            }
        }
    }

    IEnumerator PostTickets()
    {
        int idAlumno = PlayerPrefs.GetInt("IdAlumno");
        string URL = PlayerPrefs.GetString("ApiPrefix") + "alumno/" + idAlumno + "/tickets";


        Ticket ticket = new Ticket();

        if(totalAmount < 0)
        {
            totalAmount = 0;
        }

        ticket.tickets = totalAmount;

        string json = JsonUtility.ToJson(ticket);

        var req = new UnityWebRequest(URL, "PUT");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        handleResult(req);

        req.Dispose();
    }

    void handleResult (UnityWebRequest req)
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
            sc.CambiaEscena("Final");
        }
    }

}
