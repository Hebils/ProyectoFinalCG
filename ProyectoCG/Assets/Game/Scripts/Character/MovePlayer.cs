using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class MovePlayer : MonoBehaviour
{
    public float speedPlayer = 5f;
    public float speedRotation = 200f;
    public float jumpForce = 5f;
    public float jumpDelay = 2f;

    private float x;
    private float y;

    private bool isGrounded;

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

    void FixedUpdate()
    {



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
        if (context.performed && Mathf.Abs(rb.linearVelocity.y) < 0.001f && isGrounded)
        {
            animator.SetBool("IsGrounded", false);
            isGrounded = false;

            StartCoroutine(JumpDelay());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // El jugador ha tocado el suelo, puedes realizar acciones adicionales aquí si es necesario
            animator.SetBool("IsGrounded", true);
            isGrounded = true;
        }
    }

    private IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(jumpDelay);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

}
