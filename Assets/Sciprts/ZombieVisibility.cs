using UnityEngine;

public class ZombieVisibility : MonoBehaviour
{
    [Header("Flashlight")]
    public Transform flashlight;
    public float visibilityRange = 15f;
    public float visibilityAngle = 55f;

    private Renderer[] zombieRenderers;

    void Start()
    {
        // Find the player's flashlight
        if (flashlight == null)
        {
            GameObject lightObject = GameObject.Find("Flashlight");

            if (lightObject != null)
                flashlight = lightObject.transform;
        }

        zombieRenderers = GetComponentsInChildren<Renderer>();

        SetVisible(false);
    }

    void LateUpdate()
    {
        if (flashlight == null)
            return;

        Vector3 direction =
            transform.position + Vector3.up -
            flashlight.position;

        float distance = direction.magnitude;

        float angle = Vector3.Angle(
            flashlight.forward,
            direction
        );

        bool inFlashlight =
            distance <= visibilityRange &&
            angle <= visibilityAngle / 2f;

        // Check if walls block the flashlight
        if (inFlashlight)
        {
            if (Physics.Raycast(
                flashlight.position,
                direction.normalized,
                out RaycastHit hit,
                distance))
            {
                inFlashlight =
                    hit.collider.GetComponentInParent<ZombieVisibility>() == this;
            }
        }

        SetVisible(inFlashlight);
    }

    void SetVisible(bool visible)
    {
        // Hide or show the zombie model
        foreach (Renderer zombieRenderer in zombieRenderers)
        {
            if (zombieRenderer != null)
                zombieRenderer.enabled = visible;
        }
    }
}
