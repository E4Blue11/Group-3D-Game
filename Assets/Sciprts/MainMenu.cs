using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject instructionsPanel;

    void Start()
    {
        // Hide instructions when the menu opens
        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        // Start the game
        Time.timeScale = 1f;
        SceneManager.LoadScene("ZombieArena");
    }

    public void OpenInstructions()
    {
        // Show the instructions
        if (instructionsPanel != null)
            instructionsPanel.SetActive(true);
    }

    public void CloseInstructions()
    {
        // Return to the main menu
        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        // Close the game
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
