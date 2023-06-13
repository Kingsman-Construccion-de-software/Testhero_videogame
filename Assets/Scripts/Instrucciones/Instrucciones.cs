using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Instrucciones : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> imagenes;
    [SerializeField]
    private TMP_Text nombre;
    [SerializeField]
    private TMP_Text texto;

    private List<string> nombres = new List<string> {
        "MarioGame",
        "Laberinto",
        "Pesca",
        "Coches",
        "SpaceInvaders",
    };

    private List<string> textos = new List<string> {
        "Control: Utilizar las flechas derecha e izquierda y la barra espaciadora para moverse, la tecla E para el cofre. Objetivo:Tú eres el personaje zorro y tu deber es responder la pregunta que se encuentra en la parte superior y abrir el cofre cuya respuesta sea la de esa pregunta antes del límite de tiempo.",
        "Control: Utilizar las flechas  arriba, abajo derecha o izquierda  para moverse. Objetivo: Te encuentras en un laberinto con 4 caminos, tu deber es contestar la pregunta que se encuentra arriba de tí y entrar al camino en donde se encuentra la respuesta correcta antes del límite de tiempo.",
        "Control: Utilizar las flechas derecha e izquierda y la barra espaciadora para moverse, la tecla E para pescar. Objetivo: Antes del límite de tiempo, debes de pescar el pez  en el área de pesca que corresponda a la respuesta de la pregunta que se encuentra en la parte superior de la pantalla.",
        "Control: Utilizar las flechas  derecha e izquierda para moverse, arriba para acelerar y hacia abajo para frenar. Objetivo: Manejando un carro en una pista, existen 4 carriles disponibles. Tu deber es responder la pregunta que está en la parte superior y conducir al carril de la respuesta correcta antes del límite de tiempo.",
        "Control: Utilizar las flechas derecha e izquierda para moverse y la barra espaciadora para disparar.Objetivo: Te están atacando unos invasores espaciales, como buen piloto debes de esquivarlos. Asimismo, debes de responder la pregunta arriba de tí moviendote y disparando al enemigo del color de la respuesta correcta antes del límite de tiempo.",
    };
    private int currentIndex = 0;

    void Start()
    {
        UpdateData();
    }

    void UpdateData()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("ImagenJuego");

        if (currentIndex >= 0 && currentIndex < nombres.Count)
        {
            nombre.text = nombres[currentIndex];
            texto.text = textos[currentIndex];

            if (currentIndex >= 0 && currentIndex < imagenes.Count)
            {
                Image imageComponent = obj.GetComponent<Image>();

                if (imageComponent != null)
                {
                    imageComponent.sprite = imagenes[currentIndex];
                }
            }
        }
    }

    public void NextData()
    {
        currentIndex++;
        if (currentIndex >= nombres.Count)
        {
            currentIndex = 0;
        }
        UpdateData();
    }

    public void PreviousData()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = nombres.Count - 1;
        }
        UpdateData();
    }

    public void IrALogin()
    {
        SceneManager.LoadScene("Login");
    }
}
