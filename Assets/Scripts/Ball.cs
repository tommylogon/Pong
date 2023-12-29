using System;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    List<Vector2> possibleDirection = new List<Vector2>()
    {
        new Vector2(0.2f, 3.2f),  // Up
        new Vector2(0.2f, 3.2f),  // Diagonal Right Up
        new Vector2(-3.2f, 3.2f), // Diagonal Left Up
        new Vector2(0.2f, -3.2f), // Down
        new Vector2(3.2f, -3.2f), // Diagonal Right Down
        new Vector2(-3.2f, -3.2f), // Diagonal left Down
        
    };
    [SerializeField] float speedIncreaseFactor = 1.1f;
    [SerializeField] float maxSpeed = 10f;

    public Action onHit;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       
    }

   
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Paddle") || other.CompareTag("Wall"))
        {
            Vector2 normal = GameManager.instance.GetNormal(transform.position, other);
            ReflectBall(normal);

            IncreaseSpeed(other.GetComponent<Rigidbody2D>().velocity);
            onHit?.Invoke();
        }
        
    }


    private void StartDirection()
    {

        int randomIndex = UnityEngine.Random.Range(0, possibleDirection.Count);
        while (randomIndex > possibleDirection.Count)
        {
            Debug.Log("Index is " + randomIndex);
            randomIndex = UnityEngine.Random.Range(0, possibleDirection.Count);
        }

        rb.velocity = possibleDirection[randomIndex] * speedIncreaseFactor;
    }
    private void IncreaseSpeed(Vector2 SpeedAdder)
    {
        if(SpeedAdder.magnitude > 0)
        {
            //SpeedAdder.y = rb.velocity.y;
            rb.velocity += SpeedAdder;
        }
        else
        {
            rb.velocity *= speedIncreaseFactor;
        }
        
        
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }
    private void ReflectBall(Vector2 normal)
    {
        Vector2 incomingVector = rb.velocity;
        rb.velocity = Vector2.Reflect(incomingVector, normal);
        
    }

    public void ResetBall(bool startNew)
    {
        transform.position = Vector2.zero;
        if (startNew)
        {
            StartDirection();
        }
        else { rb.velocity = Vector2.zero; }
        
    }

}
