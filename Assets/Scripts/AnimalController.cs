using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    private GameObject targetHole;
    public float moveSpeed = 2.0f;

    public void SetTargetHole(GameObject hole)
    {
        targetHole = hole;
    }

    void Update()
    {
        if (targetHole != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetHole.transform.position, moveSpeed * Time.deltaTime);
        }
        // else
        // {
        //     // move randomly
        //     transform.position += new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0) * moveSpeed * Time.deltaTime;
        // }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // // disappear if the animal reaches A hole (even if it's not the target hole)
        // if (collision.CompareTag("Hole"))
        //     {
        //         // Destroy the animal
        //         Destroy(gameObject);
        //     }

        // disappear if the animal reaches THE target hole
        if (collision.gameObject == targetHole)
        {
            // Destroy the animal
            Destroy(gameObject);
        }
    }
}