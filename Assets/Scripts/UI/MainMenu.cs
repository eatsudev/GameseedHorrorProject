using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public Button optionsButton;

    private void Start()
    {
        startButton.onClick.AddListener(PlayButton);
        quitButton.onClick.AddListener(QuitButton);
        optionsButton.onClick.AddListener(OptionsButton);
    }
    public void PlayButton()
    {
        SceneManager.LoadScene("derriel");
        Time.timeScale = 1.0f;
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void OptionsButton()
    {
        SceneManager.LoadScene("Options");
    }
    
}
