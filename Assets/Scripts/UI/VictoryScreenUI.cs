using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryScreenUI : MonoBehaviour
{
    public GameObject victoryScreen;
    public Button retryButton;
    public Button backtoMenuButton;
    void Start()
    {
        retryButton.onClick.AddListener(Retry);
        backtoMenuButton.onClick.AddListener(BackToMenu);
    }

    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }
    public void HideVictoryScreen()
    {
        victoryScreen.SetActive(false);
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}
