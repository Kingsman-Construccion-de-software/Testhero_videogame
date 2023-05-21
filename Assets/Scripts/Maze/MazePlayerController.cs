using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePlayerController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;

    Animator anime;
    SpriteRenderer sp;

    private MazeManager controller;

    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        // Algo que tienes en el script 
        sp = GetComponent<SpriteRenderer>();
        // Otra cosa de escena
        controller = FindObjectOfType<MazeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.gameOver) { 
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * moveSpeed * Time.deltaTime);
            if (horizontalInput != 0 || verticalInput != 0) {
                anime.SetBool("isMoving", true);
            } else {
                anime.SetBool("isMoving", false);
            }
            if (horizontalInput < 0) {
                sp.flipX = true;
            } else {
                if (horizontalInput > 0) {
                    sp.flipX = false;
                }
            }
        }
        else
        {
            anime.SetBool("isMoving", false);
        }
    }
}
