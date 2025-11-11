using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class AdvancedPlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool isSprinting;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float jumpForce = 7f;
    public float airControl = 0.4f;
    public float rotationSpeed = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => jumpPressed = true;
        controls.Player.Sprint.performed += ctx => isSprinting = true;
        controls.Player.Sprint.canceled += ctx => isSprinting = false;
    }

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        float speed = isSprinting ? sprintSpeed : walkSpeed;
        float controlFactor = isGrounded ? 1f : airControl;

        Vector3 moveVelocity = move * speed * controlFactor;
        Vector3 targetVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
        rb.linearVelocity = targetVelocity;

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        jumpPressed = false;

        if (transform.position.y < -10f)
        {
            transform.position = new Vector3(0, 2, 0);
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}
