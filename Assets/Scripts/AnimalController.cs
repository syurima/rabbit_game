using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AnimalController : MonoBehaviour
{
    private GameObject targetHole = null;
    private Vector2 deviation = new Vector2(0, 0);
    private const float deviationUpdateInterval = 2.0f;
    public float moveSpeed = 2.0f;
    
    private const float panicDistance = 2f;
    private const float avoidDistance = 3.5f;
    
    private const float honingDistance = 3f;
    float hideTimeout = 1.0f;

    public float timeOnField = 0.0f;

    private Animator animator;
    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        animator.SetFloat("xVelocity", rb.linearVelocity.x);

        if (rb.linearVelocity.x > 0)
        {
            // flip x
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnEnable()
    {
        // Start moving when the object is enabled
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            StartCoroutine(chooseDirection());
        }
    }

    private void OnDisable()
    {
        // Stop movement when the object is disabled
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    
    private IEnumerator chooseDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            float distanceToPlayer = Vector2.Distance(transform.position, PlayerController.instance.transform.position);
            float distanceToTarget = Vector2.Distance(transform.position, targetHole.transform.position);
            
            Vector2 direction;

            // if the animal is very close to the player, move away from the player
            if (distanceToPlayer < panicDistance)
            {
                direction = (transform.position - PlayerController.instance.transform.position).normalized;
            }
            // otherwise move towards the target hole
            else if (targetHole != null)
            {
                // if the animal is far from the target hole, add deviation to the direction
                // this is to make the animal move in a more random way
                if (distanceToTarget > honingDistance) {
                    direction = ((Vector2)targetHole.transform.position - (Vector2)transform.position + deviation).normalized;
                }
                else
                {
                    direction = ((Vector2)targetHole.transform.position - (Vector2)transform.position).normalized;
                }

                // Try to go around the player if the animal is close to the player
                if (distanceToPlayer < avoidDistance)
                {
                    Vector3 rotatedDirection = Quaternion.Euler(0, 0, 90) * new Vector3(direction.x, direction.y, 0);
                    direction = new Vector2(rotatedDirection.x, rotatedDirection.y);
                }
            }
            else 
            {
                direction = Vector2.zero;
            }

            rb.linearVelocity = direction * moveSpeed;

            // if the animal is very close to the target hole, go in the hole
            if (distanceToTarget <= 0.4f && timeOnField > hideTimeout) {
                gameObject.SetActive(false);
            }
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