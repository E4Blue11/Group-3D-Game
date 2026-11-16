using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 0.15f;
    public Transform playerCamera;

    private CharacterController controller;
    private Vector3 velocity;
    private float cameraRotationX;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Find the player camera
        if (playerCamera == null)
        {
            Camera mainCamera = GetComponentInChildren<Camera>();

            if (mainCamera != null)
                playerCamera = mainCamera.transform;
        }

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Keep the cursor unlocked when the game is over
        if (Time.timeScale == 0f)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        HandleMovement();
        HandleMouseLook();

        // Unlock cursor with Escape
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Lock cursor with left click
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void HandleMovement()
    {
        if (Keyboard.current == null)
            return;

        Vector2 input = Vector2.zero;

        // WASD movement
        if (Keyboard.current.wKey.isPressed)
            input.y += 1;

        if (Keyboard.current.sKey.isPressed)
            input.y -= 1;

        if (Keyboard.current.aKey.isPressed)
            input.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1;

        input = Vector2.ClampMagnitude(input, 1f);

        Vector3 move =
            transform.right * input.x +
            transform.forward * input.y;

        // Check if the player is grounded
        bool grounded = controller.isGrounded;

        if (grounded && velocity.y < 0)
            velocity.y = -2f;

        // Jump
        if (grounded &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move the player
        Vector3 movement = move * moveSpeed;
        movement.y = velocity.y;

        controller.Move(movement * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        if (Mouse.current == null ||
            playerCamera == null ||
            Cursor.lockState != CursorLockMode.Locked)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        // Limit vertical camera rotation
        cameraRotationX -= mouseY;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -85f, 85f);

        playerCamera.localRotation =
            Quaternion.Euler(cameraRotationX, 0f, 0f);

        // Rotate the player horizontally
        transform.Rotate(Vector3.up * mouseX);
    }
}
