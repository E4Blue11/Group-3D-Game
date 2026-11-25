using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Sandevistan : MonoBehaviour
{
    [Header("Sandevistan")]
    public float duration = 7f;
    public float cooldown = 20f;
    public float slowMultiplier = 0.3f;

    [Header("UI")]
    public TMP_Text sandevistanText;
    public Slider sandevistanBar;

    public bool IsActive => isActive;

    private bool isActive;
    private float activeTimer;
    private float cooldownTimer;

    private readonly Dictionary<NavMeshAgent, float> originalSpeeds =
        new Dictionary<NavMeshAgent, float>();

    void Start()
    {
        isActive = false;
        activeTimer = 0f;
        cooldownTimer = 0f;

        UpdateUI();
    }

    void Update()
    {
        // Stop timers while paused or after death
        if (Time.timeScale == 0f)
            return;

        if (isActive)
        {
            activeTimer -= Time.deltaTime;

            if (activeTimer <= 0f)
            {
                EndSandevistan();
            }
            else
            {
                SlowZombies();
            }
        }
        else if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownTimer = Mathf.Max(0f, cooldownTimer);
        }

        // Activate with F when ready
        if (Keyboard.current != null &&
            Keyboard.current.fKey.wasPressedThisFrame &&
            !isActive &&
            cooldownTimer <= 0f)
        {
            ActivateSandevistan();
        }

        UpdateUI();
    }

    void ActivateSandevistan()
    {
        isActive = true;
        activeTimer = duration;

        // Cooldown does not start yet
        cooldownTimer = 0f;

        originalSpeeds.Clear();

        SlowZombies();
    }

    void SlowZombies()
    {
        // Slow zombies, including newly spawned ones
        NavMeshAgent[] zombies =
            FindObjectsByType<NavMeshAgent>();

        foreach (NavMeshAgent zombie in zombies)
        {
            if (zombie == null)
                continue;

            if (!originalSpeeds.ContainsKey(zombie))
            {
                originalSpeeds.Add(zombie, zombie.speed);
            }

            zombie.speed =
                originalSpeeds[zombie] * slowMultiplier;
        }
    }

    void EndSandevistan()
    {
        isActive = false;
        activeTimer = 0f;

        RestoreZombieSpeeds();

        // Begin the full cooldown after activation ends
        cooldownTimer = cooldown;
    }

    void RestoreZombieSpeeds()
    {
        foreach (var zombie in originalSpeeds)
        {
            if (zombie.Key != null)
                zombie.Key.speed = zombie.Value;
        }

        originalSpeeds.Clear();
    }

    void UpdateUI()
    {
        if (sandevistanText == null ||
            sandevistanBar == null)
            return;

        if (isActive)
        {
            // Active timer drains from full to empty
            sandevistanText.text =
                "SANDEVISTAN — ACTIVE " +
                Mathf.CeilToInt(activeTimer) + "s";

            sandevistanBar.value =
                Mathf.Clamp01(activeTimer / duration);
        }
        else if (cooldownTimer > 0f)
        {
            // Cooldown starts at 20 seconds
            sandevistanText.text =
                "SANDEVISTAN — " +
                Mathf.CeilToInt(cooldownTimer) + "s";

            sandevistanBar.value =
                1f - Mathf.Clamp01(cooldownTimer / cooldown);
        }
        else
        {
            // Ability is ready
            sandevistanText.text =
                "SANDEVISTAN — READY";

            sandevistanBar.value = 1f;
        }
    }

    void OnDisable()
    {
        if (isActive)
        {
            isActive = false;
            RestoreZombieSpeeds();
        }
    }
}
