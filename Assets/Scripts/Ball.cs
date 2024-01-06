using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] float dissolveAmount = 0f;
    [SerializeField] float dissolveDuration = 2f;
    bool isDissolving;

    private bool shouldStartNewRound;

    public event Action onHitWallOrPaddle;
    public event Action OnDissolveFinished;
    public Action<Vector2> OnBallPositionChanged;

    private Vector2 lastreportedPosition;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve speedToColorCurve;
    [SerializeField] private Light2D ballLight;

    [SerializeField] private Color startColor = Color.blue;
    [SerializeField] private Color endColor = Color.red;

    [SerializeField] private Material material;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        material = GetComponent<SpriteRenderer>().material;

        material.SetFloat("_DissolveAmount", 1);
    }

    private void Update()
    {
        if ((lastreportedPosition - (Vector2)transform.position).sqrMagnitude > 1)
        {
            lastreportedPosition = (Vector2)transform.position;
            OnBallPositionChanged?.Invoke(lastreportedPosition);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDissolving)
        {
            if (other.CompareTag("Paddle") || other.CompareTag("Wall"))
            {
                Vector2 normal = GameManager.instance.GetNormal(transform.position, other);
                ReflectBall(normal);

                IncreaseSpeed(other.GetComponent<Rigidbody2D>().velocity);
                onHitWallOrPaddle?.Invoke();

            }
        }
        

    }


    private void StartDirection()
    {
        if (shouldStartNewRound)
        {
            ResetColor();

            int randomIndex = UnityEngine.Random.Range(0, possibleDirection.Count);
            while (randomIndex > possibleDirection.Count)
            {
                Debug.Log("Index is " + randomIndex);
                randomIndex = UnityEngine.Random.Range(0, possibleDirection.Count);
            }

            rb.velocity = possibleDirection[randomIndex] * speedIncreaseFactor;
        }

    }
    private void IncreaseSpeed(Vector2 SpeedAdder)
    {
        if (SpeedAdder.magnitude > 0)
        {
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
        ColorShift();


    }

    public void ResetBall(bool startNewRound)
    {
        rb.velocity = Vector2.zero;
        Dissolve(!startNewRound);//is startnewrouns is false the ball is reset and dissolves. reversed if true.
        shouldStartNewRound = startNewRound;



    }

    private void ColorShift()
    {

        float speed = rb.velocity.magnitude;
        float t = speedToColorCurve.Evaluate(Mathf.InverseLerp(0, maxSpeed, speed));

        // Determine the color based on speed
        Color currentColor = Color.Lerp(startColor, endColor, t);

        // Apply the color to the sprite and trail
        spriteRenderer.color = currentColor;
        trailRenderer.startColor = currentColor;
        trailRenderer.endColor = currentColor;
        ballLight.color = currentColor;
    }

    private void ResetColor()
    {
        spriteRenderer.color = startColor;
        trailRenderer.startColor = startColor;
        trailRenderer.endColor = startColor;
        ballLight.color = startColor;
    }

    private void Dissolve(bool setToDissolve)
    {
        StartCoroutine(DissolveCoroutine(setToDissolve, dissolveDuration));
    }

    private IEnumerator DissolveCoroutine(bool setToDissolve, float duration)
    {
        isDissolving = setToDissolve;
        float time = 0;
        float startValue = setToDissolve ? 0f : 1f; // Assuming 0 is not dissolved and 1 is fully dissolved
        float endValue = setToDissolve ? 1f : 0f;
        material.SetFloat("_DissolveAmount", startValue); // Set initial state

        while (time < duration)
        {
            time += Time.deltaTime;
            dissolveAmount = Mathf.Lerp(startValue, endValue, time / duration);
            material.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        }

        material.SetFloat("_DissolveAmount", endValue);
        dissolveAmount = endValue;// Ensure it ends at the exact value
        transform.position = Vector2.zero;

        if (setToDissolve && dissolveAmount == 1f)
        {
            OnDissolveFinished?.Invoke(); // Notify that the dissolve effect is finished
        }
        if (!setToDissolve && dissolveAmount == 0f)
        {
            if (shouldStartNewRound)
            {
                StartDirection();
            }
        }
    }
}
