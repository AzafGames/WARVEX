
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Input System
    private InputSystem_Actions controls;
    private Vector2 moveInput;

    // Player Components
    private CharacterController playerController;
    private Animator playerAnim;

    // Movement Settings
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;

    // Jump and Gravity Settings
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravity = -9.81f;

    // Player States
    private float verticalVelocity;
    private bool isRunning;
    private bool jumpRequested;
    private bool isGrounded;


    private void Awake()
    {
        controls = new InputSystem_Actions();

        playerController = GetComponent<CharacterController>();
        playerAnim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        controls.Player.Move.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Move.Disable();
    }


    private void Update()
    {
        ReadPlayerInput();

        HandleMovement();

        HandleJump();

        ApplyGravity();

        UpdateAnimation();
    }


    // Read input from the Input System
    private void ReadPlayerInput()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        isRunning = controls.Player.Sprint.IsPressed();

        if (controls.Player.Jump.WasPressedThisFrame())
        {
            jumpRequested = true;
        }
    }


    // Handle player movement
    private void HandleMovement()
    {
        float currentSpeed;

        if (isRunning)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        Vector3 movement =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        Vector3 finalMovement = movement * currentSpeed;

        finalMovement.y = verticalVelocity;

        playerController.Move(finalMovement * Time.deltaTime);
    }


    // Handle jumping
    private void HandleJump()
    {
        isGrounded = playerController.isGrounded;

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        if (jumpRequested)
        {
            if (isGrounded)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                playerAnim.SetTrigger("Jump");
            }

            jumpRequested = false;
        }
    }


    // Apply gravity
    private void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
    }


    // Update player animations
    private void UpdateAnimation()
    {
        playerAnim.SetFloat("MoveX", moveInput.x, 0.1f, Time.deltaTime);
        playerAnim.SetFloat("MoveY", moveInput.y, 0.1f, Time.deltaTime);

        playerAnim.SetBool("Grounded", isGrounded);
    }
}
