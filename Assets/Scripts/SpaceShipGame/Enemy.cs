using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Sprite[] animationSprites;
    public float animationTime = 1.0f;
    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;
    public System.Action killed;
    public bool haveCorrectedAnswer = false;
    public int colorId = -1;
    public SpaceShipGameController controller;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    private void AnimateSprite()
    {
        _animationFrame++;
        if(_animationFrame >= this.animationSprites.Length) {
            _animationFrame = 0;
        }

        _spriteRenderer.sprite = this.animationSprites[_animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Shot")) {
            if(this.haveCorrectedAnswer && this.colorId != -1) {
                Debug.Log("Correcto");
                controller.OnCorrectAnswer(this.colorId);
            } else if(this.haveCorrectedAnswer == false &&
                (this.colorId == 0 ||
                this.colorId == 1 ||
                this.colorId == 2 ||
                this.colorId == 3)) {
                    Debug.Log("Incorrecto");
                    controller.OnWrongAnswer(this.colorId);
                }
            this.killed.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}
