using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button resumeButton;
    public Button backButton;

    public static bool isPaused;

    private void Start()
    {
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Escape.performed += TogglePause;

        pauseMenu.SetActive(false);
        resumeButton.onClick.AddListener(Resume);
        backButton.onClick.AddListener(BacktoMenu);
    }

    void Update()
    {

    }

    private void TogglePause(InputAction.CallbackContext ctx)
    {
        if (!Player_Interaction.instance.raycastInteractMode)
        {
            if (ctx.performed)
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
            
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);

        isPaused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);

        isPaused = false;
        Time.timeScale = 1f;
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}
