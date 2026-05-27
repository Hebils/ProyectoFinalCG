using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class MovePlayer : MonoBehaviour
{
    #region Movimiento
    public float speedPlayer = 5f;
    public float speedRotation = 200f;
    public float jumpForce = 5f;
    public float jumpDelay = 0.5f;
    public float jumpRunDelay = 0.25f;

    #endregion

    private float x;
    private float y;

    private bool isGrounded;
    private bool isJumpingDelayed;

    private Vector2 movementInput;

    private Animator animator;
    private Rigidbody rb;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {



    }

    void FixedUpdate()
    {
        x = Mathf.Clamp(movementInput.x, -1f, 1f);
        y = Mathf.Clamp(movementInput.y, -1f, 1f);


        transform.Rotate(0, x * speedRotation * Time.deltaTime, 0);
        transform.Translate(0, 0, y * speedPlayer * Time.deltaTime);

        if (animator != null)
        {
            animator.SetFloat("VelX", x);
            animator.SetFloat("VelY", y);

            animator.SetFloat("Blend", Mathf.Abs(x) + Mathf.Abs(y));
        }

    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        float mouseX = lookInput.x * speedRotation * Time.deltaTime;
        float mouseY = lookInput.y * speedRotation * Time.deltaTime;
        transform.Rotate(0, mouseX, 0);
        Camera.main.transform.Rotate(-mouseY, 0, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();

        if (movementInput.magnitude > 0.1f)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !isJumpingDelayed)
        {
            bool isRunningJump = movementInput.magnitude > 0.1f;
            float selectedJumpDelay = isRunningJump ? jumpRunDelay : jumpDelay;

            animator.SetBool("IsGrounded", false);
            animator.SetTrigger("IsJumping");
            isGrounded = false;
            StartCoroutine(JumpAfterDelay(selectedJumpDelay));

        }
    }

    private IEnumerator JumpAfterDelay(float delay)
    {
        isJumpingDelayed = true;
        yield return new WaitForSeconds(delay);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJumpingDelayed = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // El jugador ha tocado el suelo, puedes realizar acciones adicionales aquí si es necesario
            animator.SetBool("IsGrounded", true);
            animator.ResetTrigger("IsJumping");
            isGrounded = true;
        }
    }

}
