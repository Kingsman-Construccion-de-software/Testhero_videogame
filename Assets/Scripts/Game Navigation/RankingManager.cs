using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class RankingManager : MonoBehaviour
{
    SceneController sc;
    public Score[] scores;

    int scoreHeight = 50;
    int verticalPadding = 100;
    int initialOffset = 25;
    int verticalOffset = 50;

    [SerializeField] GameObject UIScore;
    [SerializeField] RectTransform content;
    [SerializeField] List<Sprite> badges;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<BackgroundMusic>().PlayFin();

        //datos para pruebas
        //TODO: Remover antes de deployment
        if (PlayerPrefs.GetInt("IdExamen") == -1)
        {
            sc = FindObjectOfType<SceneController>();
            return;
        }

        sc = FindObjectOfType<SceneController>();
        StartCoroutine("GetScores");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            sc.CambiaEscena("Tickets");
        }
    }

    IEnumerator GetScores()
    {
        int idExamen = PlayerPrefs.GetInt("IdExamen");
        string URL = PlayerPrefs.GetString("ApiPrefix") + "alumno/examen/" + idExamen;

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
            scores = JsonHelper.getJsonArray<Score>(req.downloadHandler.text);
            scores = scores.OrderByDescending(s => s.puntaje).ToArray();
            for(int i = 0; i<scores.Length; i++)
            {
                DisplayScores();
            }
        }
    }

    void DisplayScores()
    {
        int totalHeight = scores.Length * scoreHeight + verticalPadding;
        content.sizeDelta = new Vector2(0, totalHeight);

        int top = totalHeight / 2 - initialOffset;
        int num = 1;

        foreach(Score score in scores)
        {
            GameObject go = Instantiate(UIScore);
            go.transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform, false);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(0, top, 0);
            top -= verticalOffset;

            ScoreView sv = go.GetComponent<ScoreView>();
            string nombreCompleto = score.nombres + " " + score.apellidos;
            if(nombreCompleto.Length > 10)
            {
                nombreCompleto = nombreCompleto.Substring(0, 10) + ".";
            }
            sv.textoNombre.text = num + "." + nombreCompleto;
            sv.textoPuntos.text = score.puntaje.ToString();

            if (num <= 3)
            {
                sv.badge.gameObject.SetActive(true);
                sv.badge.sprite = badges[num - 1];
            }
            num++;
        }
    }
}
