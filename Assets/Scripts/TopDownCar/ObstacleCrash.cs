using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCrash : MonoBehaviour
{
    // Start is called before the first frame update
    public float duration = 15f;
    private float timeElapsed = 0f;
    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed < duration)
        {
            float newY = Mathf.Lerp(startY, 0f, timeElapsed / duration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            GetComponent<Road_Animation>().enabled = false; // pausa el script de road
            GetComponent<Car_Movement>().enabled = false; // pausa el script de movimiento
        }
    }
}

// private void OnTriggerEnter(Collider other)
// {
//     if (other.gameObject.CompareTag("Player"))
//     {
//         isPaused = true;
//         Time.timeScale = 0;
//     }
// }
