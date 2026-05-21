using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float runSpeed = 20f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float limitVerticalRotation = 80f;
    [SerializeField] private bool lockMouse = true;
    [SerializeField] private Transform cameraTransform;

    private Rigidbody rb;
    private Vector2 moveInput;
    private float currentSpeed;
    private float verticalRotation;
    private bool isGrounded;
    private bool lookEnabled = true;

    public Animator playerAnim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        playerAnim = GetComponent<Animator>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (lockMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Start()
    {

        playerAnim.SetBool("IsGrounded", true);

    }

    private void FixedUpdate()
    {
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y) * currentSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        playerAnim.SetFloat("VelX", moveInput.x);
        playerAnim.SetFloat("VelY", moveInput.y);
        playerAnim.SetFloat("Blend", moveInput.magnitude);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!lookEnabled)
        {
            return;
        }

        if (cameraTransform == null)
        {
            return;
        }

        Vector2 lookInput = context.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * lookInput.x);

        verticalRotation -= lookInput.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -limitVerticalRotation, limitVerticalRotation);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    public void SetLookEnabled(bool enabled)
    {
        lookEnabled = enabled;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        currentSpeed = context.ReadValueAsButton() ? runSpeed : walkSpeed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            playerAnim.SetBool("IsGrounded", false);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            playerAnim.SetBool("IsGrounded", true);
        }
    }
}
