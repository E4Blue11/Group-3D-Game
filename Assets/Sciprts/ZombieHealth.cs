using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    [Header("Health")]
    public int health = 6;

    [Header("Health and Ammo Drops")]
    public GameObject healthPackPrefab;
    public GameObject ammoPackPrefab;

    public float healthDropChance = 0.15f;
    public float ammoDropChance = 0.20f;

    [Header("Weapon Drops")]
    public GameObject assaultRiflePickupPrefab;
    public GameObject shotgunPickupPrefab;

    public float weaponDropChance = 0.15f;

    private bool isDead;

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        health -= damage;

        if (health <= 0)
        {
            isDead = true;
            TryDropItem();
            Destroy(gameObject);
        }
    }

    void TryDropItem()
    {
        // Choose which item to drop
        float dropRoll = Random.value;

        if (dropRoll < healthDropChance &&
            healthPackPrefab != null)
        {
            Instantiate(
                healthPackPrefab,
                transform.position,
                Quaternion.identity
            );
        }
        else if (dropRoll <
            healthDropChance + ammoDropChance &&
            ammoPackPrefab != null)
        {
            Instantiate(
                ammoPackPrefab,
                transform.position,
                Quaternion.identity
            );
        }
        else if (dropRoll <
            healthDropChance + ammoDropChance + weaponDropChance)
        {
            // Randomly choose a weapon
            GameObject weaponPrefab =
                Random.value < 0.5f
                    ? assaultRiflePickupPrefab
                    : shotgunPickupPrefab;

            if (weaponPrefab != null)
            {
                Instantiate(
                    weaponPrefab,
                    transform.position,
                    Quaternion.identity
                );
            }
        }
    }
}
