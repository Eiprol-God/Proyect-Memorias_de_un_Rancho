using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Ajustes de movimiento")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 8f;

    [Header("Detección de piso")]
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
    }

    void Update()
    {
        // Movimiento horizontal
        moveInput = Input.GetAxisRaw("Horizontal");

        // Detectar correr
        isRunning = Input.GetKey(KeyCode.LeftShift) && moveInput != 0;

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetBool("Jump", true);   // Activar animación de salto
        }

        // Control de animaciones
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        // Comprobar suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Movimiento según si corre o camina
        float speed = isRunning ? runSpeed : moveSpeed;
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Resetear Jump al tocar el piso
        if (isGrounded && rb.linearVelocity.y <= 0.1f)
            anim.SetBool("Jump", false);
    }

    void UpdateAnimations()
    {
        // Walking
        bool walking = (moveInput != 0 && !isRunning && isGrounded);
        anim.SetBool("Walking", walking);

        // Run
        bool run = (moveInput != 0 && isRunning && isGrounded);
        anim.SetBool("Run", run);

        // Idle
        bool idle = (moveInput == 0 && isGrounded);
        // Idle es automático porque Walking y Run estarán en false
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