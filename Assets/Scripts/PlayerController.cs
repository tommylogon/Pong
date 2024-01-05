using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public bool isAI;
    [SerializeField] int playerID;
    [SerializeField] public string playerName;
    [SerializeField] float speedModifier;
    [SerializeField] float speedBase;
    [SerializeField] float shrinkFactor; //how much smaller the paddle is based on score
    [SerializeField] float currentScale;

    [SerializeField] PlayerInput playerInput;
    [SerializeField] NPCInput npcInput;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }
    private void OnEnable()
    {
        npcInput.OnMovementPerformed += NpcInput_OnMovementPerformed;
    }

    private void NpcInput_OnMovementPerformed(object sender, NPCInput.OnMovementPerformedEventArgs e)
    {
        if (isAI)
        {
            rb.velocity = e.velocity * (speedBase + speedModifier);
        }

        
    }

    public void MoveLeft()
    {
        if (!isAI)
        {

                   
                rb.velocity = Vector2.left * (speedBase + speedModifier);
                
            
        }
    }
    public void MoveRight()
    {
        if (!isAI)
        {

        
            
                rb.velocity = Vector2.right * (speedBase + speedModifier);
                
            
            
        }
    }

    public void Reset()
    {
       // transform.localScale = new Vector3(5,1,1);
        transform.position = new Vector2(0,transform.position.y);
        rb.velocity = Vector2.zero ;

        if (isAI)
        {
            npcInput.StartAI(true);
        }
        else
        {
            npcInput.StartAI(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            rb.velocity = Vector2.zero;
            Vector2 resetPos = new Vector2(transform.position.x + GameManager.instance.GetNormal(transform.position, other).x / 10, transform.position.y);
            transform.position = resetPos;
            
        }
    }

    public void Shrink(int score)
    {
        transform.localScale = new Vector3(Mathf.Clamp(5 - score,1,5), 1, 1);
        speedModifier = score;
        
    }

    public int GetPlayerID()
    {
        return playerID;
    }
}
