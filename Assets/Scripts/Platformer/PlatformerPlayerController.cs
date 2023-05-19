using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerController : MonoBehaviour
{

    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float jumpPower = 10.0f;

    private Rigidbody2D rb2d;
    private SpriteRenderer spr;
    private Animator anim;
    public LayerMask groundMask;

    private PlatformerManager pc;

    private bool colidingChest = false;
    private GameObject chest;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pc = FindObjectOfType<PlatformerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pc.gameOver)
        {

            Move();

            if (Input.GetButton("Jump") && IsGrounded())
                Jump();

            if (Input.GetKeyDown(KeyCode.E) && colidingChest)
            {
                
                Chest c = chest.GetComponent<Chest>();
                if (c.isCorrectAnswer())
                {
                    pc.OnCorrectAnswer(c.IdRespuesta);
                }
                else
                {
                    pc.OnWrongAnswer(c.IdRespuesta);
                }
                
            }
        }
    }

    private void Move()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        if(horizontalInput == -1)
        {
            spr.flipX = true;
            anim.SetBool("isMoving", true);
        } else if (horizontalInput == 1)
        {
            spr.flipX = false;
            anim.SetBool("isMoving", true);
        } else
        {
            anim.SetBool("isMoving", false);
        }
        rb2d.velocity = new Vector2(horizontalInput * playerSpeed, rb2d.velocity.y);
    }

    private void Jump()
    {
        rb2d.velocity = new Vector2(0, jumpPower);
    }

    private bool IsGrounded()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 1.5f, groundMask))
        {
            return true;
        }
        return false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
        {
            colidingChest = false;
            chest = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
        {
            colidingChest = true;
            chest = collision.gameObject;
        }
    }


}
