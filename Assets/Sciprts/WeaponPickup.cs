using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Weapon")]
    public WeaponType weaponType = WeaponType.Shotgun;

    [Header("Pickup Lifetime")]
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
            // Equip the collected weapon
            shooting.CollectWeapon(weaponType);
            Destroy(gameObject);
        }
    }
}
