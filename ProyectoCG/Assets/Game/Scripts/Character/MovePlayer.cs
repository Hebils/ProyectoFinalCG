using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class MovePlayer : MonoBehaviour
{
    #region Movimiento
    public float speedPlayer = 5f;
    public float speedRotation = 200f;
    public float minVerticalLook = -45f;
    public float maxVerticalLook = 70f;
    public float jumpForce = 5f;
    public float jumpDelay = 0.5f;
    public float jumpRunDelay = 0.25f;
    private float x;
    private float y;
    private bool isGrounded;
    private bool isJumpingDelayed;
    private Vector2 movementInput;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 _playerPosition;
    private Transform cameraTransform;
    private float cameraPitch;
    public Transform respawnPoint;
    private PlayaController playaController;
    private SelvaController selvaController;
    #endregion

    void Awake()
    {
        if (respawnPoint != null)
        {
            _playerPosition = respawnPoint.position;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        PrepararCamara();
        BloquearCursor(SceneManager.GetActiveScene().name != "Menu");
        BuscarControladoresEscena();
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

        ActualizarSonidoPasos();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();

        if (lookInput.sqrMagnitude > 0.01f && SceneManager.GetActiveScene().name != "Menu")
        {
            BloquearCursor(true);
        }

        if (cameraTransform == null)
        {
            PrepararCamara();
        }

        float mouseX = lookInput.x * speedRotation * Time.deltaTime;
        float mouseY = lookInput.y * speedRotation * Time.deltaTime;

        transform.Rotate(0, mouseX, 0);

        if (cameraTransform != null)
        {
            cameraPitch = Mathf.Clamp(cameraPitch - mouseY, minVerticalLook, maxVerticalLook);
            Vector3 cameraAngles = cameraTransform.localEulerAngles;
            cameraAngles.x = cameraPitch;
            cameraTransform.localEulerAngles = cameraAngles;
        }
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

    void ActualizarSonidoPasos()
    {
        if (GameManager.Instance == null) return;

        if (movementInput.magnitude > 0.1f && isGrounded)
        {
            GameManager.Instance.StartFootsteps();
        }
        else
        {
            GameManager.Instance.StopFootsteps();
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StopFootsteps();
        }

        BloquearCursor(false);
    }

    void OnTriggerEnter(Collider other)
    {
        BuscarControladoresEscena();

        if (other.CompareTag("Recolectable"))
        {
            if (playaController != null)
            {
                playaController.RecogerObjeto(other.gameObject);
            }
            else if (selvaController != null)
            {
                selvaController.RecogerObjeto(other.gameObject);
            }
        }

        if (other.CompareTag("Final"))
        {
            if (respawnPoint != null)
            {
                transform.position = respawnPoint.position;
            }
        }

        if (other.CompareTag("Helicoptero") && selvaController != null)
        {
            selvaController.TerminarSelva();
        }
    }

    void BuscarControladoresEscena()
    {
        if (playaController == null)
        {
            playaController = FindFirstObjectByType<PlayaController>();
        }

        if (selvaController == null)
        {
            selvaController = FindFirstObjectByType<SelvaController>();
        }
    }

    void PrepararCamara()
    {
        if (Camera.main == null) return;

        cameraTransform = Camera.main.transform;
        cameraPitch = NormalizarAngulo(cameraTransform.localEulerAngles.x);
        cameraPitch = Mathf.Clamp(cameraPitch, minVerticalLook, maxVerticalLook);
        Vector3 cameraAngles = cameraTransform.localEulerAngles;
        cameraAngles.x = cameraPitch;
        cameraTransform.localEulerAngles = cameraAngles;
    }

    float NormalizarAngulo(float angle)
    {
        return angle > 180f ? angle - 360f : angle;
    }

    void BloquearCursor(bool bloqueado)
    {
        Cursor.lockState = bloqueado ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !bloqueado;
    }

}
