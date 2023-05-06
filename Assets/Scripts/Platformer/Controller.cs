using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float jumpPower = 10.0f;

    private Rigidbody2D rb2d;
    private SpriteRenderer spr;
    private Animator anim;
    public LayerMask groundMask;

    private GameManager gameManager;

    private bool colidingChest = false;
    private GameObject chest;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetButton("Jump") && IsGrounded())
            Jump();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (colidingChest)
            {
                chest.GetComponent<Chest>().Open();
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
