using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // -------- MOVIMIENTO --------
    public float speed = 5f;
    public float runSpeed = 9f;
    public float idleDelay = 3f;
    private float moveInput;
    private float idleTimer = 0f;
    private bool isRunning;

    // -------- SALTO --------
    public float jumpForce = 25f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    // -------- SLIDE / DERRAPE --------
    public float slideSpeed = 10f;
    public float slideDuration = 0.4f;
    private bool isSliding = false;
    private float slideTimer;

    // -------- COMPONENTES --------
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D boxCollider;

    // -------- COLLIDER SLIDE --------
    [Header("Configuración del Collider para Slide")]
    [Tooltip("El nuevo tamaño que tendrá el BoxCollider2D al deslizarse.")]
    public Vector2 slideColliderSize = new Vector2(0.8f, 0.5f);
    [Tooltip("La nueva posición (offset) que tendrá el BoxCollider2D al deslizarse.")]
    public Vector2 slideColliderOffset = new Vector2(0f, -0.7f);
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Guardar las dimensiones originales del collider
        originalColliderSize = boxCollider.size;
        originalColliderOffset = boxCollider.offset;
    }

    void Update()
    {
        GroundCheck();
        HandleMovement();
        HandleJump();
        HandleSlideInput();
        UpdateSlideState();
    }

    // ============================
    //        MOVIMIENTO
    // ============================
    void HandleMovement()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // Velocidad según si corre o camina
        float currentSpeed = isRunning ? runSpeed : speed;

        // Si NO está deslizando, puede moverse
        if (!isSliding)
        {
            rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
        }

        // Flip del personaje
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // Idle timer
        if (moveInput == 0)
            idleTimer += Time.deltaTime;
        else
            idleTimer = 0;

        // Animaciones de movimiento
        if (isSliding) return; // No cambiar animaciones mientras desliza

        if (moveInput != 0)
        {
            anim.SetBool("Idle_static", false);
            anim.SetBool("Idle_Mov", false);

            if (isRunning)
            {
                anim.SetBool("Walking", false);
                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Walking", true);
                anim.SetBool("Run", false);
            }
        }
        else
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Run", false);

            if (idleTimer >= idleDelay)
            {
                anim.SetBool("Idle_static", false);
                anim.SetBool("Idle_Mov", true);
            }
            else
            {
                anim.SetBool("Idle_static", true);
                anim.SetBool("Idle_Mov", false);
            }
        }
    }

    // ============================
    //           SALTO
    // ============================
    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        anim.SetBool("Jump", !isGrounded);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // ============================
    //         SLIDE / DERRAPE
    // ============================
    void HandleSlideInput()
    {
        // Activa el slide con 'LeftControl' solo si está en el suelo y no está ya deslizando.
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && !isSliding)
        {
            StartSlide();
        }
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        anim.SetBool("Slide", true);

        // Cambiar collider para el slide
        if (boxCollider != null)
        {
            boxCollider.size = slideColliderSize;
            boxCollider.offset = slideColliderOffset;
        }
    }

    void UpdateSlideState()
    {
        if (isSliding)
        {
            // Aplica velocidad horizontal constante durante el slide, en la dirección que mira el personaje
            float slideDirection = transform.localScale.x;
            rb.linearVelocity = new Vector2(slideDirection * slideSpeed, rb.linearVelocity.y);

            slideTimer -= Time.deltaTime;

            if (slideTimer <= 0f)
            {
                // Terminar el slide y restaurar el collider
                isSliding = false;
                anim.SetBool("Slide", false);

                if (boxCollider != null)
                {
                    boxCollider.size = originalColliderSize;
                    boxCollider.offset = originalColliderOffset;
                }
            }
        }
    }

    // Gizmo para GroundCheck
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
