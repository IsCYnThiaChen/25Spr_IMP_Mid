using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum MovingPatterns { Straight, Lerp, Teleport, EaseIn, EaseInAndOut }
    [SerializeField]
    private MovingPatterns movingPatterns;
    [SerializeField]
    private GameManager gameManager;

    private Vector3 currentTarget;
    private Vector3 startPos;
    private float movementTimer = 0;

    private float moveSpeed = 10f; // Speed of movement
    private Vector3 moveDirection = Vector3.zero; // Current movement direction


    // Player shoot bullets
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletSpawnPoint;

    private float lastShotTime = -1f;
    private float shootCooldown = 1f;

    private GameObject bulletPreview;

    public AudioSource audioSource;
    public AudioClip soundClip;


    private void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            gameManager.score++;
        }
    }

    private void Update()
    {
        // Handle WASD movement
        HandleMovementInput();

        // Move player based on the calculated direction
        Vector3 newPos = transform.position + moveDirection * moveSpeed * Time.deltaTime;

        // Apply new position to the player's transform
        transform.position = newPos;

        // Update rotation to face the move direction (if any movement is happening)
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        }

        HandleShooting();
    }

    /// <summary>
    /// Handles movement input for WASD keys
    /// </summary>
    private void HandleMovementInput()
    {
        moveDirection = Vector3.zero;  // Reset the direction every frame

        // Detect input from WASD keys and set the corresponding movement direction
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector3.forward; // Move forward
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += Vector3.back; // Move backward
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector3.left; // Move left
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right; // Move right
        }

        moveDirection.Normalize(); // Normalize to avoid faster movement when pressing multiple keys
    }

    /// <summary>
    /// Handles shooting mechanics when space bar is pressed
    /// </summary>
    private void HandleShooting()
    {
        // Check if the space bar is pressed and the cooldown has passed
        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastShotTime >= shootCooldown)
        {
            // Shoot a bullet
            ShootBullet();
            audioSource.PlayOneShot(soundClip);

            // Update the last shot time
            lastShotTime = Time.time;
        }
    }

    /// <summary>
    /// Instantiates and shoots a bullet
    /// </summary>
    private void ShootBullet()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {

            // Get the mouse position on the screen
            Vector3 mousePosition = Input.mousePosition;
            Debug.Log(mousePosition);

            // Convert the mouse position to a point in the world
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            // Calculate the direction towards the mouse (along the ray)
            Vector3 targetDirection = ray.direction;

            // Instantiate the bullet at the spawn point and set its rotation to a fixed value (no rotation)
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity); // No rotation
            
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                // Add a force to the bullet to make it move (this assumes the bullet prefab has a Rigidbody)
                bulletRb.AddForce(-Vector3.forward * 20f, ForceMode.Impulse); // Adjust the force and speed as needed
            }

            StartCoroutine(DestroyBulletAfterTime(bullet, 4f));
        }
    }
    // <summary>
    /// Destroys the bullet after a certain amount of time.
    /// </summary>
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bullet); // Destroy the bullet after the specified time
    }
}
