using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

    [SerializeField]
    private AudioClip inicio;

    [SerializeField]
    private List<AudioClip> minijuegos;

    [SerializeField]
    private AudioClip puntos;

    [SerializeField]
    private AudioClip fin;

    private AudioSource aus;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        aus = GetComponent<AudioSource>();
        aus.loop = true;
        aus.playOnAwake = true;
    }

    public void PlayInicio()
    {
        aus.Stop();
        aus.clip = inicio;
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
