using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour

{
    public bool haveCorrectedAnswer = false;
    public int colorId = -1;
    public SpaceShipGameController controller;
    public float animationTime = 2.0f;
    private int _animationFrame;
    [SerializeField]
    private GameObject explosion;

    void Start()
    {
        InvokeRepeating("AnimateSprite", this.animationTime, this.animationTime);
    }

    private void AnimateSprite()
    {
        _animationFrame++;
        if (_animationFrame > 4)
        {
            _animationFrame = 0;
        }
    }

    private void Update()
    {
        if (_animationFrame == 1)
        {
            Quaternion target = Quaternion.Euler(40f, 40f, 40f);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5f);
        }
        else if (_animationFrame == 2)
        {
            Quaternion target = Quaternion.Euler(0f, 0f, 0f);
            transform.rotation =  Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 2f);
        }
        else if (_animationFrame == 3)
        {
            Quaternion target = Quaternion.Euler(-40f, -40f, -40f);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 2f);
        }
        else if (_animationFrame == 4)
        {
            Quaternion target = Quaternion.Euler(0f, 0f, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Shot")) {
            if(this.haveCorrectedAnswer && this.colorId != -1) {
                controller.OnCorrectAnswer(this.colorId);
            } else if(this.haveCorrectedAnswer == false &&
                (this.colorId == 0 ||
                this.colorId == 1 ||
                this.colorId == 2 ||
                this.colorId == 3)) {
                    controller.OnWrongAnswer(this.colorId);
                }

            Explode();
        }
    }


    public void Explode()
    {
        GameObject go = Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(go, 1.0f);
        Destroy(gameObject);
        this.gameObject.SetActive(false);
    }
}
