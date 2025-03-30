using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public InputActionReference movement;
    public float moveSpeed = 5.0f;

    public static PlayerController instance;

    private Animator animator;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

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

        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);

        if (rb.linearVelocity.x < 0)
        {
            // flip x
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Animal"))
        {
            // set animal as inactive
            collision.gameObject.SetActive(false);

            GameController.instance.OnAnimalCaught();
        }
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
    }
}