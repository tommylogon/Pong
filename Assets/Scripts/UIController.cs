using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] string baseText;
    // Start is called before the first frame update

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
        UpdateScore(0, 0);
        baseText = scoreText.text;
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
        while(duration > 0)
        {
            countDownText.text = duration.ToString();
            yield return new WaitForSeconds(1);
            duration--;
        }
        countDownText.text = "";
    }
}
