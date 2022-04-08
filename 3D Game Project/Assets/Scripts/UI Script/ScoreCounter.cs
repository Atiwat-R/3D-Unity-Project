using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{

    public Text FinalScore;
    public Text HighScore;


    void Start()
    {
        float toDisplay = PlayerPrefs.GetFloat("FinalScore");
        float highScore = PlayerPrefs.GetFloat("HighScore"); // Initially 0, but gets updated

        // Keep High Score
        if (toDisplay > highScore) {
            PlayerPrefs.SetFloat("HighScore", toDisplay);
        }

        // Show this Game's Score
        FinalScore.text = "Score:" + Mathf.Round(toDisplay);
        // Show HighScore
        HighScore.text = "High Score:" + Mathf.Round(PlayerPrefs.GetFloat("HighScore"));
    }


}
