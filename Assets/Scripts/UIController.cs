using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] string baseText;

    [SerializeField] GameObject FocusMessagePanel;
    [SerializeField] GameObject MainMenu;


    private void Start()
    {
        
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("UICONTROLLER CONFLICT");
        }

        countDownText.text = string.Empty;
        gameOverText.text = string.Empty;
        FocusMessagePanel.SetActive(false);
        UpdateScore(0, 0);
        
    }

    public void ToggleMainMenu()
    {
        MainMenu.SetActive(!MainMenu.activeSelf);
        Time.timeScale = MainMenu.activeSelf ? 0 : 1;
    }

    public void ToggleLobby()
    {
        Debug.Log("No lobby yet");
    }



    public void UpdateScore(int p1, int p2)
    {
        string text = baseText;
        text = text.Replace("{P1Score}", p1.ToString());
        text = text.Replace("{P2Score}", p2.ToString());
        
        scoreText.text = text;
    }
    public void StartCountdown(int duration)
    {
        StartCoroutine(CountdownCoroutine(duration));
    }
    private IEnumerator CountdownCoroutine(int duration)
    {
        FocusMessagePanel.SetActive(true);
        while(duration > 0)
        {
            countDownText.text = duration.ToString();
            yield return new WaitForSeconds(1);
            duration--;
        }
        countDownText.text = "";
        FocusMessagePanel.SetActive(false);
    }
    public void ShowGameOver(string playerName)
    {
        gameOverText.text = "WINNER: <br>" + playerName;
        FocusMessagePanel.SetActive(true);
    }
    
    
}
