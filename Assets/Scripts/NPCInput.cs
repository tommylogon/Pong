using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class NPCInput : MonoBehaviour
{
    public enum NPC_TYPE
    {
        Random,
        Defender,
        Smasher
    }
    [SerializeField] bool isActive;
    [SerializeField] bool enableRandomAI;
    [SerializeField] NPC_TYPE type;
    [SerializeField] float timer;
    [SerializeField] int smasherDistance;
    [SerializeField] Ball ballReff;

    public event EventHandler<OnMovementPerformedEventArgs> OnMovementPerformed;

    public class OnMovementPerformedEventArgs: EventArgs
    {
        public Vector2 velocity;
    }

    private void OnEnable()
    {
        ballReff.OnBallPositionChanged += DefenderMovement;
        ballReff.OnBallPositionChanged += SmasherMovement;
    }

    private void OnDisable()
    {
        ballReff.OnBallPositionChanged -= DefenderMovement;
        ballReff.OnBallPositionChanged -= SmasherMovement;
    }

    private void Update()
    {
        if(isActive)
        {
            if (type == NPC_TYPE.Random)
            {
                RandomMovement();
            }
           
        }
        
    }

    private IEnumerator WaitForMovement(float timer)
    {
        yield return new WaitForSeconds(timer);
        OnMovementPerformed?.Invoke(this, new OnMovementPerformedEventArgs { velocity = MoveInput()});
    }

    public Vector2 MoveInput()
    {
        Vector2 newspeed = Vector2.zero;
        newspeed.x = Random.Range(-1, 1);
        return newspeed;
    }


    public void StartAI(bool startAI)
    {
        isActive = startAI;
        if (enableRandomAI)
        {
            SetRandomAIType();
        }
        else
        {
            type = NPC_TYPE.Defender;
        }
        

    }



    private void RandomMovement()
    {
        if (timer <= 0 && isActive) 
        {
            float randomTimer = Random.Range(0.1f, 1f);            
            StartCoroutine( WaitForMovement(randomTimer));
            timer = randomTimer;

        }

        timer -= Time.deltaTime;
    }

    private void DefenderMovement(Vector2 ballPos)
    {
        if (isActive && type == NPC_TYPE.Defender)
        {
            Vector2 newSpeed = Vector2.zero;
            newSpeed.x = Mathf.Sign(ballPos.x - transform.position.x);

            OnMovementPerformed?.Invoke(this, new OnMovementPerformedEventArgs { velocity = newSpeed }) ;
        }
    }

    private void SmasherMovement(Vector2 ballPos)
    {
        if (isActive && type == NPC_TYPE.Smasher)
        {
            Vector2 newSpeed = Vector2.zero;
            if(Vector2.Distance(transform.position, ballPos) > smasherDistance)
            {
                newSpeed.x = Mathf.Sign(ballPos.x - transform.position.x) * -1;
            }
            else
            {
                newSpeed.x = Mathf.Sign(ballPos.x - transform.position.x);

            }
            

            OnMovementPerformed?.Invoke(this, new OnMovementPerformedEventArgs { velocity = newSpeed });
        }
        
    }

    private void SetRandomAIType()
    {
        // Get all values from the NPC_TYPE enum
        Array aiTypes = Enum.GetValues(typeof(NPC_TYPE));

        // Select a random index
        int randomIndex = UnityEngine.Random.Range(0, aiTypes.Length);

        // Set the AI type
        type = (NPC_TYPE)aiTypes.GetValue(randomIndex);

        // Reset the timer for Random movement type
        if (type == NPC_TYPE.Random)
        {
            timer = 0;
        }
    }
}
