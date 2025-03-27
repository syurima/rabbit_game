using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    private GameObject targetHole;
    public float moveSpeed = 2.0f;
    
    private float panicDistance = 3.5f;

    public void SetTargetHole(GameObject hole)
    {
        targetHole = hole;
    }

    void Update()
    {
        // if near the player, run away
        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < panicDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, -moveSpeed * Time.deltaTime);
        }

        // otherwise move towards the target hole
        else if (targetHole != null)
        {
            // //add deviation to the target hole
            // Vector3 deviation = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
            // transform.position = Vector3.MoveTowards(transform.position, targetHole.transform.position + deviation, moveSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetHole.transform.position, moveSpeed * Time.deltaTime);
        }
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