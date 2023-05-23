using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExamCompletedController : MonoBehaviour
{

    [SerializeField]
    TMP_Text nombreTexto;

    // Start is called before the first frame update
    void Start()
    {
        nombreTexto.text = PlayerPrefs.GetString("TituloExamen");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
