using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public TextMeshPro timerText;
    public TextMeshPro scoreText;
    public GameObject gameOverPanel;

    public static UI instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            this.gameOverPanel.SetActive(false);
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
        if (Mathf.Round(GameController.instance.timeRemaining) < 10)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.white;
        }

        scoreText.text = "Score: " + GameController.instance.score.ToString();

        if (GameController.instance.gameOver)
        {
            gameOverPanel.SetActive(true);
        }
    }
}