using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionReference movement;
    public float moveSpeed = 5.0f;

    public static PlayerController instance;

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
        transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * moveSpeed * Time.deltaTime;
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
}