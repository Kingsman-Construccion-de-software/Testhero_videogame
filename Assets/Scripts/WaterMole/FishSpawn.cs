using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject eKey;
    private GameObject eKeyInstance;

    public GameObject player;
    public GameObject interactSpritePrefab;
    public Transform interactSpriteParent;
    public float interactDistance = 2f;
    private GameObject currentInteractSprite;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer interactSpriteRenderer;
    private Vector3 initialPosition;
    public float spinSpeed = 100f;
    public Transform pivotPoint;
    public float verticalSpeed = 2f;
    public float verticalDistance = 0.5f;
    public float minDisappearTime = 1f;
    public float maxDisappearTime = 5f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        interactSpriteRenderer = interactSpritePrefab.GetComponent<SpriteRenderer>();
        initialPosition = transform.position;
        StartCoroutine(SpinAndDisappearCoroutine());
    }

    private void Update()
    {
        // Check if the player is close to the sprite with a collider
        if (Vector3.Distance(player.transform.position, transform.position) <= interactDistance)
        {
            // Spawn the interact sprite if it doesn't exist
            if (currentInteractSprite == null)
            {
                currentInteractSprite = Instantiate(interactSpritePrefab, interactSpriteParent);
                currentInteractSprite.transform.position = transform.position + Vector3.up * 2f;
            }

            // Check if the player presses the "e" key
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Spawn another sprite above the player
                GameObject newSprite = Instantiate(interactSpritePrefab, interactSpriteParent);
                newSprite.transform.position = player.transform.position + Vector3.up * 2f;
            }
        }
        else
        {
            // Destroy the interact sprite if the player moves away
            if (currentInteractSprite != null)
            {
                Destroy(currentInteractSprite);
                currentInteractSprite = null;
            }
        }
    }

    private IEnumerator SpinAndDisappearCoroutine()
    {
        while (true)
        {
            // Set sprite renderers to enabled
            spriteRenderer.enabled = true;
            interactSpriteRenderer.enabled = true;

            // Set initial rotation and position
            transform.rotation = Quaternion.identity;
            transform.position = initialPosition;

            // Get a random disappear time
            float disappearTime = Random.Range(minDisappearTime, maxDisappearTime);
            float rotationAngle = 0f;

            while (disappearTime > 0f)
            {
                // Spin the sprite around the pivot point
                rotationAngle += spinSpeed * Time.deltaTime;
                transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

                // Move the sprite vertically
                float yOffset = Mathf.Sin(Time.time * verticalSpeed) * verticalDistance;
                transform.position = initialPosition + new Vector3(0f, yOffset, 0f);

                disappearTime -= Time.deltaTime;
                yield return null;
            }

            // Disable the sprite renderers
            spriteRenderer.enabled = false;
            interactSpriteRenderer.enabled = false;

            // Get a random wait time before the next iteration
            float waitTime = Random.Range(minDisappearTime, maxDisappearTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
