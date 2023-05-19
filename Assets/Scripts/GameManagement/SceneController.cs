using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
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