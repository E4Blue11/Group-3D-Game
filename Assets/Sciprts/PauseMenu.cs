using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;

    private bool isPaused;

    void Start()
    {
        // Hide the pause menu
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        // Toggle pause with Escape
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (Time.timeScale == 0f && !isPaused)
                return;

            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        // Restart the arena
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void QuitToMenu()
    {
        // Return to the main menu
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu");
    }
}
