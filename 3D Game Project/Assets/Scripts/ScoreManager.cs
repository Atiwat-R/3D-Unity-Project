using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;

    public float scoreCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + Mathf.Round(scoreCount);

        // Save Score across Scenes
        PlayerPrefs.SetFloat("FinalScore", scoreCount);
    }

    public void AddScore(float points) {
        scoreCount += points;
    }
}
