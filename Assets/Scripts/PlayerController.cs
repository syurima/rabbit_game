using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction controls;
    public float moveSpeed = 5.0f;

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        Vector2 inputDirection = controls.ReadValue<Vector2>();
        transform.position += new Vector3(inputDirection.x, inputDirection.y, 0) * moveSpeed * Time.deltaTime;
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