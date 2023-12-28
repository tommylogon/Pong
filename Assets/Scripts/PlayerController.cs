using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool isAI;
    [SerializeField] int minY;
    [SerializeField] int maxY;
    [SerializeField] float speedModifier;
    [SerializeField] float shrinkFactor; //how much smaller the paddle is based on score
    [SerializeField] float currentScale;

    [SerializeField] PlayerInput playerInput;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void MoveLeft(InputAction.CallbackContext context)
    {
        if (isAI)
        {

        }
        else
        {
            
                rb.velocity = Vector2.left * speedModifier;
                
            
        }
    }
    public void MoveRight(InputAction.CallbackContext context)
    {
        if (isAI)
        {

        }
        else
        {
            
                rb.velocity = Vector2.right * speedModifier;
                
            
            
        }
    }

    public void Reset()
    {
       // transform.localScale = new Vector3(5,1,1);
        transform.position = new Vector2(0,transform.position.y);
        rb.velocity = Vector2.zero ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            rb.velocity = Vector2.zero;
            Vector2 resetPos = new Vector2(transform.position.x + GameManager.instance.GetNormal(transform.position, collision).x / 10, transform.position.y);
            transform.position = resetPos;
            //rb.velocity = GameManager.instance.GetNormal(collision);
        }
        
        
    }

    public void Shrink(int score)
    {
        transform.localScale = new Vector3(Mathf.Clamp(5 - score,1,5), 1, 1);
        speedModifier += score;
        Debug.Log(shrinkFactor);
    }
}
