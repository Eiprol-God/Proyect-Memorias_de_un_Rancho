using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Ajustes de movimiento")]
    public float moveSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpForce = 8f;

    [Header("Slide")]
    public float slideDuration = 0.4f;
    private float slideTimer;
    private bool isSliding;

    [Header("Detección de suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;

    private bool isGrounded;
    private float moveInput;
    private bool isRunning;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.gravityScale = 3f;
    }

    void Update()
    {
        // --- Movimiento horizontal ---
        moveInput = Input.GetAxisRaw("Horizontal");

        // Correr
        isRunning = (Input.GetKey(KeyCode.LeftShift) && moveInput != 0);

        // Saltar (solo si está en el suelo)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetBool("Jump", true);
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.Z) && !isSliding)
        {
            anim.SetTrigger("Attack");
        }

        // Slide (Shift + S)
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            StartSlide();
        }

        // Slide Jump (Shift + W)
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            StartSlide();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Terminar slide
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
            {
                isSliding = false;
                anim.SetBool("Slide", false);
            }
        }

        // Terminar animación de salto
        if (isGrounded && anim.GetBool("Jump"))
        {
            anim.SetBool("Jump", false);
        }

        // Actualizar animaciones
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        // Comprobar suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Movimiento
        if (!isSliding)
        {
            float speed = isRunning ? runSpeed : moveSpeed;
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        }
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        anim.SetBool("Slide", true);
    }

    void UpdateAnimations()
    {
        anim.SetBool("Walking", moveInput != 0 && !isRunning && isGrounded);
        anim.SetBool("Run", moveInput != 0 && isRunning && isGrounded);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}