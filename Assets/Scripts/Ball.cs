using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    List<Vector2> possibleDirection = new List<Vector2>()
    {
        new Vector2(0.2f, 1.2f),  // Up
        new Vector2(0.2f, 1.2f),  // Diagonal Right Up
        new Vector2(-1.2f, 1.2f), // Diagonal Left Up
        new Vector2(0.2f, -1.2f), // Down
        new Vector2(1.2f, -1.2f), // Diagonal Right Down
        new Vector2(-1.2f, -1.2f), // Diagonal left Down
        
    };
    [SerializeField] float speedIncreaseFactor = 1.1f;
    [SerializeField] float currentSpeed;
    [SerializeField] float maxSpeed = 10f;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartDirection();
    }

    private void Update()
    {
        currentSpeed = rb.velocity.magnitude;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Paddle") || other.CompareTag("Wall"))
        {
            Vector2 normal = CalculateReflectionNormal(other);
            ReflectBall(normal);
            // Handle any additional interactions, like speed increase
        }
    }


    private void StartDirection()
    {

        int randomIndex = Random.Range(0, possibleDirection.Count);
        while (randomIndex > possibleDirection.Count)
        {
            Debug.Log("Index is " + randomIndex);
            randomIndex = Random.Range(0, possibleDirection.Count);
        }

        rb.velocity = possibleDirection[randomIndex] * speedIncreaseFactor;
    }
    private void IncreaseSpeed()
    {
        rb.velocity *= speedIncreaseFactor;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }
    private void ReflectBall(Vector2 normal)
    {
        Vector2 incomingVector = rb.velocity;
        rb.velocity = Vector2.Reflect(incomingVector, normal);
        IncreaseSpeed();
    }
    private Vector2 CalculateReflectionNormal(Collider2D collider)
    {
        if (collider.CompareTag("Wall"))
        {
            // For vertical walls
            if (transform.position.x > collider.transform.position.x)
            {
                // Ball is to the right of the wall, so the wall is likely on the left
                return new Vector2(1, 0); // Normal pointing to the right
            }
            else
            {
                // Ball is to the left of the wall, so the wall is likely on the right
                return new Vector2(-1, 0); // Normal pointing to the left
            }
        }
        else if (collider.CompareTag("Paddle"))
        {
            // For horizontal paddles
            if (transform.position.y > collider.transform.position.y)
            {
                // Ball is above the paddle, so the paddle is likely the bottom one
                return new Vector2(0, 1); // Normal pointing upwards
            }
            else
            {
                // Ball is below the paddle, so the paddle is likely the top one
                return new Vector2(0, -1); // Normal pointing downwards
            }
        }

        return Vector2.zero; // Default case, should not usually happen
    }
}
