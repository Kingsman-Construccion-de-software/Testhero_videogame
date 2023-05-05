using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager2 : MonoBehaviour
{
    public static void cambiaEscena(string escena)
    {
        SceneManager.LoadScene("Login_2");
    }

    public static void cambiaEscena2(string escena)
    {
        SceneManager.LoadScene("Login_3");
    }

    public static void cambiaEscena3(string escena)
    {
        SceneManager.LoadScene("Login_1");
    }

    public static void salir()
    {
        Application.Quit();
    }


    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

}