using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;


    [SerializeField]
    private AudioClip inicio;

    [SerializeField]
    private List<AudioClip> minijuegos;

    [SerializeField]
    private AudioClip tienda;

    [SerializeField]
    private AudioClip puntos;

    [SerializeField]
    private AudioClip fin;

    private AudioSource aus;


    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            aus = GetComponent<AudioSource>();
            aus.loop = true;
            aus.volume = 0.7f;
            aus.playOnAwake = true;
            instance = this;
        } 
    }

    public void PlayInicio()
    {
        aus.Stop();
        aus.clip = inicio;
        aus.time = 3f;
        aus.Play();
    }

    public void PlayTienda()
    {
        aus.Stop();
        aus.clip = tienda;
        aus.time = 3f;
        aus.Play();
    }

    public void PlayPuntos()
    {
        aus.Stop();
        aus.clip = puntos;
        aus.time = 10f;
        aus.Play();
    }

    public void PlayFin()
    {
        aus.Stop();
        aus.clip = fin;
        aus.Play();
    }

    public void PlayMinigame(int index)
    {
        aus.Stop();
        aus.clip = minijuegos[index];
        aus.Play();
    }

}
