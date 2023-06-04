using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    public Projectile laserPrefab;
    public float speed = 10.0f;
    private bool laserActive;
    private Animator anim;
    private SpaceShipGameController controller;
    [SerializeField] private GameObject explosion;
    private float minX = -19f;
    private float maxX = 19f;


    void Start()
    {
        anim = GetComponent<Animator>();
        controller = FindObjectOfType<SpaceShipGameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            float newX = transform.position.x - this.speed * Time.deltaTime;
            newX = Mathf.Clamp(newX, minX, maxX);
            this.transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            anim.SetFloat("movement", -speed* Time.deltaTime); 
        } else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            float newX = transform.position.x + this.speed * Time.deltaTime;
            newX = Mathf.Clamp(newX, minX, maxX);
            this.transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            anim.SetFloat("movement", speed * Time.deltaTime);
        } else
        {
            anim.SetFloat("movement", 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            Shoot();
        }
    }

    private void Shoot() {
        if(!laserActive) {
            Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            projectile.destroyed += LaserDestroyed;
            laserActive = true;
        }
    }

    private void LaserDestroyed() {
        laserActive = false;
    }

    public void Explode()
    {
        GameObject go = Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(go, 1.0f);
        Destroy(gameObject);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Missile") && !controller.gameOver) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
    }
}
