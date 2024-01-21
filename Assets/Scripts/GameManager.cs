using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int currentScorePlayer1; 
    int currentScorePlayer2;

    [SerializeField] int waitTimer;
    [SerializeField] ScoreZone Player1Score;
    [SerializeField] ScoreZone Player2Score;
    [SerializeField] PlayerController player1;
    [SerializeField] PlayerController player2;
    [SerializeField] Ball ball;
    [SerializeField] public GameState currentGameState;
    
    
    public enum GameState
    {
        Running,
        Paused,
        GameOver,
        Respawningball,

    }
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        UIController.Instance.UpdateScore(currentScorePlayer1, currentScorePlayer2);
        currentGameState = GameState.Paused;

        ball.ResetBall(false);


    }
    private void OnEnable()
    {
        Player1Score.onScore += () => AddScore(player1.GetPlayerID());
        Player2Score.onScore += () => AddScore(player2.GetPlayerID());
        ball.onHitWallOrPaddle += () => SoundManager.instance.PlayHitSound();
        ball.OnDissolveFinished += OnDissolveFinishedHandler;
    }

    private void OnDisable()
    {
        Player1Score.onScore -= () => AddScore(0);
        Player2Score.onScore -= () => AddScore(1);
        ball.onHitWallOrPaddle -= () => SoundManager.instance.PlayHitSound();
        ball.OnDissolveFinished -= OnDissolveFinishedHandler;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) 
        {
            SceneManager.LoadScene("GameScene");
        }
    }
    private void AddScore(int playerID)
    {
        if(currentGameState == GameState.Running)
        {
           
            if (playerID == 0)
            {
                currentScorePlayer1++;
                player1.Shrink(currentScorePlayer1);
            }
            else
            {
                currentScorePlayer2++;
                player2.Shrink(currentScorePlayer2);
            }

            UIController.Instance.UpdateScore(currentScorePlayer1, currentScorePlayer2);
            SoundManager.instance.PlayScoreSound();
            if (currentScorePlayer1 > 4 || currentScorePlayer2 > 4)
            {
                currentGameState = GameState.GameOver;
                NewRound();
                return;
                
            }

            currentGameState = GameState.Respawningball;
            NewRound();

        }
        
    }

    private void NewRound()
    {
        if(currentGameState == GameState.Running)
        {
            player1.Reset();
            player2.Reset();
            StartCoroutine(StartMatchAfterDelay());
        }
        else if(currentGameState == GameState.Respawningball)
        {
            ball.ResetBall(false);            
        }
        else if(currentGameState == GameState.GameOver)
        {
            SoundManager.instance.PlayGameOverSound();
            if(currentScorePlayer1 > 4)
            {
                UIController.Instance.ShowGameOver(player1.playerName);
            }
            else if(currentScorePlayer2 > 4)
            {
                UIController.Instance.ShowGameOver(player2.playerName);
            }
            
            ball.ResetBall(false);
        }

        
    }

    

    private void NewMatch()
    {
        currentGameState = GameState.Respawningball;
        currentScorePlayer1 = 0;
        currentScorePlayer2 = 0;
        player1.Shrink(currentScorePlayer1);
        player2.Shrink(currentScorePlayer2);
        OnDissolveFinishedHandler();


    }

    public void StartLocalMatch()
    {
        player1.isAI = false;
        player2.isAI = false;
        NewMatch();
        
        

    }

    public void StartVersusAI()
    {
        player1.isAI = false;
        player2.isAI = true;
        NewMatch();
        
        
    }

    private IEnumerator StartMatchAfterDelay()
    {
        SoundManager.instance.PlayCountDownSound();
        UIController.Instance.StartCountdown(waitTimer);
        ball.ResetBall(true);
        yield return new WaitForSeconds(waitTimer);

        
    }


    private void OnDissolveFinishedHandler()
    {
        if(currentGameState == GameState.Respawningball)
        {
            currentGameState = GameState.Running;
            NewRound();
        }
    }

    public Vector2 GetNormal(Vector2 position, Collider2D collider)
    {

        if (collider.CompareTag("Wall"))
        {
            // For vertical walls
            if (position.x > collider.transform.position.x)
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
            if (position.y > collider.transform.position.y)
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
