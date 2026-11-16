using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 5;
    public float lifetime = 7f;

    void Start()
    {
        // Remove the pickup after a few seconds
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            // Restore player health
            player.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
