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

    private Rigidbody2D rb;

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

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!GameController.instance.gameOver)
        {
            Move();
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

    void OnDisable()
    {
        // Stop movement when the object is disabled
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}