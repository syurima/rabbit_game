using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public InputActionReference movement;
    public InputActionReference catchAnimalAction;
    public float moveSpeed = 5.0f;

    public static PlayerController instance;

    private Animator animator;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    public LayerMask animalLayer;

    public Transform attackPoint;
    public float attackRange = 0.7f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Enable the input actions
        movement.action.Enable();
        catchAnimalAction.action.Enable();
        catchAnimalAction.action.performed += ctx => PlayerController.instance.CatchAnimal();
        
    }

    void Update()
    {
        if (!GameController.instance.gameOver)
        {
            Move();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        
    }

    void FixedUpdate()
    {
        // Update the animator with the current velocity
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);

        // Flip the sprite based on the direction of movement
        if (rb.linearVelocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    void Move()
    {
        Vector2 moveDirection = movement.action.ReadValue<Vector2>();
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    public void CatchAnimal()
    {

        animator.SetTrigger("CatchAnimal");

        // Check if the player is in the right position to catch an animal
        attackPoint.position = new Vector3(transform.position.x, transform.position.y, 0f) + (Vector3)rb.linearVelocity.normalized * 0.5f;
        // Check for animals within the attack range
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, animalLayer);

        if (hits.Length > 0)
        {
            foreach (Collider2D hit in hits)
            {
                Debug.Log("Hit: " + hit.gameObject.name);

                if (hit.CompareTag("Animal"))
                {
                    // Set animal as inactive
                    hit.gameObject.SetActive(false);

                    GameController.instance.OnAnimalCaught();
                }
            }
        }
        else
        {
            Debug.Log("No animals found within range.");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void PlayFootstepSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }


    void OnDisable()
    {
        // Stop movement when the object is disabled
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        movement.action.Disable();
        catchAnimalAction.action.Disable();
        catchAnimalAction.action.performed -= ctx => PlayerController.instance.CatchAnimal();
    }
}