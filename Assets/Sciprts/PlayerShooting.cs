using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public enum WeaponType
{
    Pistol,
    AssaultRifle,
    Shotgun
}

public class PlayerShooting : MonoBehaviour
{
    [Header("Weapon")]
    public Camera playerCamera;

    private WeaponType currentWeapon;
    private int damage;
    private float range;
    private float fireRate;

    [Header("Ammo")]
    private int magazineSize;
    private int currentAmmo;
    private int reserveAmmo;
    private int maxReserveAmmo;
    private float reloadTime;

    [Header("UI")]
    public TMP_Text ammoText;

    private float nextFireTime;
    private bool isReloading;
    private float reloadTimer;

    void Start()
    {
        // Find the player camera
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();

        EquipWeapon(WeaponType.Pistol);
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null)
            return;

        // Switch to pistol when out of ammo
        if (currentWeapon != WeaponType.Pistol &&
            currentAmmo <= 0 &&
            reserveAmmo <= 0)
        {
            EquipWeapon(WeaponType.Pistol);
        }

        // Update reload timer
        if (isReloading)
        {
            reloadTimer -= Time.deltaTime;

            if (reloadTimer <= 0f)
                FinishReload();

            return;
        }

        // Reload with R
        if (Keyboard.current.rKey.wasPressedThisFrame)
            Reload();

        // Automatically reload an empty magazine
        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        // Shoot with left click
        if (Mouse.current.leftButton.isPressed &&
            Time.time >= nextFireTime &&
            Cursor.lockState == CursorLockMode.Locked)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;
        nextFireTime = Time.time + fireRate;
        UpdateAmmoUI();

        // Shoot from the center of the camera
        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        // Check if the bullet hits something
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            ZombieHealth zombie =
                hit.collider.GetComponentInParent<ZombieHealth>();

            if (zombie != null)
                zombie.TakeDamage(damage);
        }
    }

    public void CollectWeapon(WeaponType weapon)
    {
        EquipWeapon(weapon);
    }

    void EquipWeapon(WeaponType weapon)
    {
        currentWeapon = weapon;

        // Set weapon stats
        switch (currentWeapon)
        {
            case WeaponType.Pistol:
                damage = 2;
                range = 6f;
                fireRate = 0.35f;

                magazineSize = 12;
                reserveAmmo = 0;
                maxReserveAmmo = 0;
                reloadTime = 1.2f;
                break;

            case WeaponType.AssaultRifle:
                damage = 2;
                range = 8f;
                fireRate = 0.12f;

                magazineSize = 30;
                reserveAmmo = 150;
                maxReserveAmmo = 150;
                reloadTime = 1.5f;
                break;

            case WeaponType.Shotgun:
                damage = 6;
                range = 4f;
                fireRate = 0.8f;

                magazineSize = 6;
                reserveAmmo = 24;
                maxReserveAmmo = 24;
                reloadTime = 1.8f;
                break;
        }

        currentAmmo = magazineSize;
        isReloading = false;
        reloadTimer = 0f;
        nextFireTime = Time.time;

        UpdateAmmoUI();
    }

    void Reload()
    {
        if (isReloading || currentAmmo >= magazineSize)
            return;

        // Switch to pistol if the weapon has no ammo
        if (currentWeapon != WeaponType.Pistol &&
            reserveAmmo <= 0)
        {
            EquipWeapon(WeaponType.Pistol);
            return;
        }

        isReloading = true;
        reloadTimer = reloadTime;
    }

    void FinishReload()
    {
        // Pistol has unlimited reserve ammo
        if (currentWeapon == WeaponType.Pistol)
        {
            currentAmmo = magazineSize;
        }
        else
        {
            int ammoNeeded = magazineSize - currentAmmo;
            int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);

            currentAmmo += ammoToLoad;
            reserveAmmo -= ammoToLoad;
        }

        isReloading = false;
        reloadTimer = 0f;

        UpdateAmmoUI();
    }

    public void AddAmmo(int amount)
    {
        // Pistol does not need ammo pickups
        if (currentWeapon == WeaponType.Pistol)
            return;

        reserveAmmo = Mathf.Min(
            reserveAmmo + amount,
            maxReserveAmmo
        );

        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText == null)
            return;

        // Display unlimited pistol ammo
        if (currentWeapon == WeaponType.Pistol)
            ammoText.text = currentAmmo + " / ∞";
        else
            ammoText.text = currentAmmo + " / " + reserveAmmo;
    }
}
