using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float directionalJumpForce = 4f; // extra push for diagonal jumps
    private Rigidbody rb;
    private Vector2 moveInput;
    private PlayerControls controls;
    private bool isGrounded = true;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => Jump();
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!isGrounded) return;

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        if (!isGrounded) return;

        Vector3 jumpDirection = Vector3.up * jumpForce;

        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        if (moveDir.magnitude > 0.1f)
        {
            jumpDirection += moveDir * directionalJumpForce;
        }

        rb.AddForce(jumpDirection, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void Update()
    {
        if (transform.position.y < -10)
        {
            transform.position = new Vector3(0, 2, 0);
            rb.linearVelocity = Vector3.zero;
        }
    }
}
