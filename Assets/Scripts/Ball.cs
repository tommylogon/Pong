using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    List<Vector2> possibleDirection = new List<Vector2>()
    {
        new Vector2(0, 1),  // Up
        new Vector2(1, 1),  // Diagonal Right Up
        new Vector2(-1, 1), // Diagonal Left Up
        new Vector2(0, -1), // Down
        new Vector2(1, -1), // Diagonal Right Down
        new Vector2(-1, -1), // Diagonal left Down
        
    };
    [SerializeField] float speedIncreaseFactor = 1;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartDirection();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            IncreaseSpeed();
            ReflectDirection(collision.contacts[0].normal);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            ReflectDirection(collision.contacts[0].normal);
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
    }
    private void ReflectDirection(Vector2 normal)
    {
        Vector2 incomingVector = rb.velocity;
        rb.velocity = Vector2.Reflect(incomingVector, normal);
    }

}
