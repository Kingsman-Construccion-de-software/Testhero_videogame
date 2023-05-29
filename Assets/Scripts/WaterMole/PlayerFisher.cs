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

    void Start()
    {
        player = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        if (inputHorizontal != 0)
        {
            Vector3 movement = new Vector3(inputHorizontal * moveSpeed * Time.deltaTime, 0f, 0f);
            transform.Translate(movement);
        }

        if (inputHorizontal > 0 && faceRight)
        {
            Flip();
        }
        else if (inputHorizontal < 0 && !faceRight)
        {
            Flip();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
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