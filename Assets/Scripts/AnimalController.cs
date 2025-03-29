using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AnimalController : MonoBehaviour
{
    private GameObject targetHole = null;
    private Vector3 deviation = new Vector3(0, 0, 0);
    private const float deviationUpdateInterval = 1.0f;
    public float moveSpeed = 2.0f;
    
    private const float panicDistance = 3.5f;
    
    private const float honingDistance = 2f;
    float hideTimeout = 2.0f;

    public float timeOnField = 0.0f;


    public Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartCoroutine(UpdateDeviation());
    }

    private IEnumerator UpdateDeviation()
    {
        while (true)
        {
            yield return new WaitForSeconds(deviationUpdateInterval);
            deviation = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        }
    }
    public void SetTargetHole(GameObject hole)
    {
        targetHole = hole;
    }

    void Update()
    {
        timeOnField += Time.deltaTime;

        if (PlayerController.instance != null && Vector2.Distance(transform.position, PlayerController.instance.transform.position) < panicDistance)
        {
            // Move away from the player + 30 deg
            Vector2 direction = (transform.position - PlayerController.instance.transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
        // otherwise move towards the target hole
        else if (targetHole != null)
        {
            // // move towards the target hole (with deviation if further than honingDistance)
            // if (Vector3.Distance(transform.position, targetHole.transform.position) > honingDistance) {
            //     rb.linearVelocity = Vector2.MoveTowards(transform.position, targetHole.transform.position + deviation, moveSpeed * Time.deltaTime);
            // }
            // else
            // {
            //     rb.linearVelocity = Vector2.MoveTowards(transform.position, targetHole.transform.position, moveSpeed * Time.deltaTime);
            // }

            Vector2 direction = ((Vector2)targetHole.transform.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
        else 
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        // disappear if the animal reaches A hole (even if it's not the target hole)
        if (collision.CompareTag("Hole") && timeOnField > hideTimeout)
        {
            gameObject.SetActive(false);
        }
    }
}