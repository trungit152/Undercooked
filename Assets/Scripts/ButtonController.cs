using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject blur;

    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SinglePlayButton()
    {
        PlayerPrefs.SetInt("PlayMode", 1);
        SceneManager.LoadScene("SelectLevel");
    }
    public void MultiPlayButton()
    {
        PlayerPrefs.SetInt("PlayMode", 2);
        SceneManager.LoadScene("SelectLevel");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void PauseButton()
    {
        if(FeelingTool.instance != null) {        
            blur.SetActive(true);
            FeelingTool.instance.ZoomIn(pausePanel.transform, 0.1f, true);
        }
    }
    public void ClosePauseButton()
    {
        if (FeelingTool.instance != null)
        {
            Time.timeScale = 1;
            blur.SetActive(false);
            FeelingTool.instance.ZoomOut(pausePanel.transform);
        }
    }

    private void TurnOffMusic()
    {

    }
}
