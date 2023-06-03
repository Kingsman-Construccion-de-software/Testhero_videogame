using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FishSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject eKey;

    public float interactDistance = 2f;
    private GameObject currentInteract;

    private SpriteRenderer spriteRenderer;

    public int idRespuesta;
    public bool esCorrecta = false;

    private Rigidbody2D rb2d;
    [SerializeField] private float velocity = 2.5f;
    [SerializeField] private float rotationSpeed = 1.0f;
    private bool jumping = false;

    private Vector3 initialPosition;
    public float minDisappearTime = 1f;
    public float maxDisappearTime = 5f;

    private MoleManager manager;
    public bool selected = false;
    public bool stopped = false;

    [SerializeField] private GameObject winObject;
    [SerializeField] private GameObject loseObject;
    [SerializeField] private GameObject splashObject;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        initialPosition = transform.position;

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0f;

        manager = FindObjectOfType<MoleManager>();

        StartCoroutine(WaitRandomTime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the fish is showing instantiate the key upon collision
        if (spriteRenderer.enabled && collision.CompareTag("Player"))
        {
            Vector2 offset = new Vector2(0, 1f);
            Vector2 position = (Vector2)transform.position + offset;
            currentInteract = Instantiate(eKey, position, Quaternion.identity);
            currentInteract.transform.parent = transform;
            currentInteract.GetComponent<SpriteRenderer>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision )
    {
        //in case the player stays on the collider when the fish dissapears
        //if the fish is active and the key is null instantiate it
        if (spriteRenderer.enabled && currentInteract == null && collision.CompareTag("Player"))
        {
            Vector2 offset = new Vector2(0, 1f);
            Vector2 position = (Vector2)transform.position + offset;
            currentInteract = Instantiate(eKey, position, Quaternion.identity);
            currentInteract.transform.parent = transform;
            currentInteract.GetComponent<SpriteRenderer>();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //when exiting the collider if the fish is active and there is a key, destroy it
        if (spriteRenderer.enabled && currentInteract != null && collision.CompareTag("Player"))
        {
            Destroy(currentInteract);
        }
    }

    private void Update()
    {

        if (manager.gameOver)
        {
            if (!stopped)
            {
                Stop();
            }
        }

        //in case the fish dissapears, make the key disappear too
        if (!spriteRenderer.enabled && currentInteract != null)
            {
                Destroy(currentInteract);
            }

            //if jumping change angle
            if (jumping)
            {
                Quaternion target = Quaternion.Euler(0f, 0f, -90f);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotationSpeed);
                //if the fish has moved on the x axis and reached its original position in y (finished jumping) then hide
                if (transform.position.x > initialPosition.x + 0.5f && transform.position.y - initialPosition.y < 0.01f)
                {
                    Hide();
                }
            }

            //if the fish is active and the player close enough
            if (spriteRenderer.enabled && currentInteract != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    selected = true;
                    if (esCorrecta)
                    {
                        manager.OnCorrectAnswer(idRespuesta);
                    }
                    else
                    {
                        manager.OnWrongAnswer(idRespuesta);
                    }
                }
            }


    }


    //reset position and rotation, enable the sprite and jump
    private void Jump()
    {
        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
        spriteRenderer.enabled = true;
        rb2d.AddForce(new Vector3(0.5f, 2f, 0f) * velocity, ForceMode2D.Impulse);
        rb2d.gravityScale = 0.5f;
        jumping = true;
        GameObject go = Instantiate(splashObject, transform.position, Quaternion.identity);
        Destroy(go, 1.0f);
    }

    //disable the sprite and the physics
    private void Hide()
    {
        jumping = false;
        spriteRenderer.enabled = false;
        rb2d.gravityScale = 0f;
        rb2d.velocity = Vector2.zero;
        GameObject go = Instantiate(splashObject, transform.position, Quaternion.identity);
        Destroy(go, 1.0f);
        if (!manager.gameOver)
        {
            StartCoroutine(WaitRandomTime());
        }
    }

    //stop when game is over
    private void Stop()
    {

        stopped = true;

        //disable collider and ekey
        if(currentInteract != null)
        {
            Destroy(currentInteract);
        }
        GetComponent<Collider2D>().enabled = false;


        if (!selected)
        {
            if (spriteRenderer.enabled)
            {
                    Hide();
            }
        } else
        {
            //stop
            rb2d.gravityScale = 0f;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0f; 
            ShowFeedback();
        }
    }

    public void ShowFeedback()
    {
        if (esCorrecta)
        {
            Vector2 offset = new Vector2(0, 1f);
            Vector2 position = (Vector2)transform.position + offset;
            Instantiate(winObject, position, Quaternion.identity);
        }
        else
        {
            Vector2 offset = new Vector2(0, 1f);
            Vector2 position = (Vector2)transform.position + offset;
            Instantiate(loseObject, position, Quaternion.identity);
        }
    }

    //wait for a random amount of seconds
    private IEnumerator WaitRandomTime()
    {
        float time = Random.Range(minDisappearTime, maxDisappearTime);
        yield return new WaitForSeconds(time);
        Jump();

    }

}

