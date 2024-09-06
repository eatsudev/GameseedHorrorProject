using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Opening : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("derriel");
        Time.timeScale = 1.0f;
    }
}
