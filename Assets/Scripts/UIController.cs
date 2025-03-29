using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI caughtText;
    public GameObject gameOverPanel;

    public static UIController instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            this.gameOverPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        int time = (int)Mathf.Round(GameController.instance.timeRemaining);
        // format the time to display as MM:SS
        timerText.text = string.Format("{0:00}:{1:00}", time / 60, time % 60);
        if (time < 10)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.white;
        }

        scoreText.text = "Score: " + (GameController.instance.score).ToString();
        caughtText.text = "Caught: " + (GameController.instance.caughtCount).ToString() + "/" + (GameController.instance.targetCount).ToString();
        gameOverPanel.SetActive(GameController.instance.gameOver);
    }

    // public void GameOver()
    // {
    //     gameOverPanel.SetActive(true);
    // }
}