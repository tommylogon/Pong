using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] TextMeshProUGUI scoreText;
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
    }

    public void UpdateScore(int p1, int p2)
    {
        string text = baseText;
        text = text.Replace("{P1Score}", p1.ToString());
        text = text.Replace("{P2Score}", p2.ToString());
        scoreText.text = text;
    }
}
