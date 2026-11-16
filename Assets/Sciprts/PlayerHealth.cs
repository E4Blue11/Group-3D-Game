using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public TMP_Text healthText;
    public GameObject gameOverPanel;
    public TMP_Text finalWaveText;

    private bool isDead;

    void Start()
    {
        Time.timeScale = 1f;
        currentHealth = maxHealth;
        isDead = false;

        // Hide the game over screen
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        // Reduce player health
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthUI();
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead)
            return;

        // Restore health without exceeding the maximum
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthUI();
    }

    void Die()
    {
        Debug.Log("Player Died");

        // Display the final wave
        WaveManager waveManager =
            FindAnyObjectByType<WaveManager>();

        if (finalWaveText != null && waveManager != null)
        {
            finalWaveText.text =
                "Wave Reached: " + waveManager.CurrentWave;
        }

        // Show the game over screen
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Stop the game
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // Restart the current scene
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    void UpdateHealthUI()
    {
        // Update the health counter
        if (healthText != null)
            healthText.text = "HP: " + currentHealth;
    }
}
