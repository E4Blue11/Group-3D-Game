using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 12;
    public float lifetime = 7f;

    void Start()
    {
        // Remove the pickup after a few seconds
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerShooting shooting =
            other.GetComponentInParent<PlayerShooting>();

        if (shooting != null)
        {
            // Add ammo to the player's reserve
            shooting.AddAmmo(ammoAmount);
            Destroy(gameObject);
        }
    }
}
