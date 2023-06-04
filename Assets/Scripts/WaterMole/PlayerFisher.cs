using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFisher : MonoBehaviour
{
    public float moveSpeed = 8;
    float inputHorizontal;
    float inputVertical;
    bool faceRight = true;
    private SpriteRenderer player;
    private float minX = -11f;
    private float maxX = 11f;
    private float minY = -8f;
    private float maxY = 4f;

    private MoleManager manager;

    private 

    void Start()
    {
        player = GetComponent<SpriteRenderer>();
        manager = FindObjectOfType<MoleManager>();
    }

    void Update()
    {

        if (!manager.gameOver)
        {

            inputHorizontal = Input.GetAxisRaw("Horizontal");
            inputVertical = Input.GetAxisRaw("Vertical");

            if (inputHorizontal != 0)
            {
                Vector3 movement = new Vector3(inputHorizontal * moveSpeed * Time.deltaTime, 0f, 0f);
                transform.Translate(movement);
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX),
                    transform.position.y,
                    transform.position.z);
            }

            if (inputVertical != 0)
            {
                Vector3 movement = new Vector3(0f, inputVertical * moveSpeed * Time.deltaTime, 0f);
                transform.Translate(movement);
                transform.position = new Vector3(transform.position.x,
                    Mathf.Clamp(transform.position.y, minY, maxY),
                    transform.position.z);
            }

            if (inputHorizontal > 0 && faceRight)
            {
                Flip();
            }
            else if (inputHorizontal < 0 && !faceRight)
            {
                Flip();
            }
        }
        
    }

    void Flip()
    {
        Vector3 currentScale = player.transform.localScale;
        currentScale.x *= -1;
        player.transform.localScale = currentScale;
        faceRight = !faceRight;
    }
}