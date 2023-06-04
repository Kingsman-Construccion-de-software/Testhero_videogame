using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject eKey;
    private GameObject eKeyInstance;

    private SpriteRenderer spr;
    [SerializeField] private GameObject openAnimation;
    private bool opened = false;

    [SerializeField] private Sprite treasure;
    [SerializeField] private Sprite empty;
    [SerializeField] private bool correctAnswer;
    [SerializeField] private GameObject gem;
    [SerializeField] private GameObject skull;


    public int IdRespuesta { get; set; }

    public void ToggleCorrectAnswer()
    {
        correctAnswer = true;
    }

    public bool isCorrectAnswer()
    {
        return correctAnswer;
    }

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!opened)
        {
            Vector2 offset = new Vector2(0, 0f);
            Vector2 position = (Vector2)transform.position + offset;
            eKeyInstance = Instantiate(eKey, position, Quaternion.identity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (eKeyInstance != null)
        {
            Destroy(eKeyInstance);
        }
    }

    public void Open()
    {
        if (!opened)
        {
            opened = true;
            if (correctAnswer)
            {
                spr.sprite = treasure;
            } else
            {
                spr.sprite = empty;
            }
            if (eKeyInstance != null)
            {
                Destroy(eKeyInstance);
            }
            this.transform.position += new Vector3(0, 0.2f, 0);
            GameObject anim = Instantiate(openAnimation, transform.position, Quaternion.identity);
            Destroy(anim, 0.25f);
            Invoke("ShowFeedback", 0.5f);
        }
    }

    public void ShowFeedback()
    {
        Vector2 offset = new Vector2(0, 1.5f);
        Vector2 position = (Vector2)transform.position + offset;
        if (correctAnswer)
        {
            Instantiate(gem, position, Quaternion.identity);
        } else
        {
            Instantiate(skull, position, Quaternion.identity);
        }
    }

}
