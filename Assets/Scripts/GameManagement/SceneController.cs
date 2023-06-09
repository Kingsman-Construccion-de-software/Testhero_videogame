using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static SceneController instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    public void CambiaEscena(string scene)
    {
          SceneManager.LoadScene(scene);
    }

    public void Salir()
    {
        Application.Quit();
    }


}