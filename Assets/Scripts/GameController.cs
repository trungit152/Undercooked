using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private float time;
    private long score;
    public int kpi;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject blur;
    [SerializeField] private TextMeshProUGUI scoreTextEndGame;
    [SerializeField] private TextMeshProUGUI status;
    [SerializeField] private GameObject nbar;
    [SerializeField] private GameObject player2;
    public static GameController instance;
    private bool isDone;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        if(PlayerPrefs.GetInt("PlayMode") == 1)
        {
            player2.SetActive(false);
        }
        else if(PlayerPrefs.GetInt("PlayMode") == 2)
        {
            player2.SetActive(true);
        }
        
    }
    private void Start()
    {
        SetStartValue();
        UpdateScoreText();
        isDone = false;
    }

    private void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
        }
        else if(!isDone)
        {
            time = 0;
            isDone = true;
            ShowPanel();
        }
        UpdateTimeText();
    }

    private void SetStartValue()
    {
        time = 300;
        score = 0;
    }
    public void GetScore(int point)
    {
        score += point;
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        scoreText.text = $"SCORE: {score}/{kpi}";
    }
    private void UpdateTimeText()
    {
        int minute = (int)time / 60;
        int second = (int)time % 60;
        string secondText = second >= 10 ? second.ToString() : '0' + second.ToString();
        timeText.text = minute.ToString() + ":" + secondText;
    }
    
    public void ShowPanel()
    {
        gamePanel.SetActive(true);
        blur.SetActive(true);
        nbar.SetActive(false);
        Time.timeScale = 0;
        if (score >= kpi)
        {
            status.text = "WIN";
        }
        else
        {
            status.text = "LOSE";
        }
        scoreTextEndGame.text = $"SCORE: {score}";
        FeelingTool.instance.ZoomIn(gamePanel.transform);
    }
    public void ClosePanel()
    {
        gamePanel.SetActive(false);
        blur.SetActive(false);
        FeelingTool.instance.ZoomOut(gamePanel.transform);
    }
}
