using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamara : MonoBehaviour
{
     [SerializeField]
    float verticalOffset = 8f;
     [SerializeField]
    float horizontalOffset = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.name == "TriggerRight"){
            Camera.main.transform.Translate(horizontalOffset,0,0);
        }else if(gameObject.name == "TriggerUp"){
            Camera.main.transform.Translate(0,verticalOffset ,0);
        }else if(gameObject.name == "TriggerLeft"){
                Camera.main.transform.Translate(-horizontalOffset,0,0);
        }else{
            Camera.main.transform.Translate(0,-verticalOffset ,0);
        }
    }
}
