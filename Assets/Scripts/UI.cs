using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text timerText;
    public Text scoreText;
    public GameObject endGamePanel;
    public Text endGameText;

    public static UI instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        timerText.text = "Time: " + Mathf.Round(GameController.instance.timeRemaining).ToString();
        scoreText.text = "Score: " + GameController.instance.score.ToString();

        if (GameController.instance.gameOver)
        {
            endGamePanel.SetActive(true);
            endGameText.text = "Game Over!";
            
        }
    }
}