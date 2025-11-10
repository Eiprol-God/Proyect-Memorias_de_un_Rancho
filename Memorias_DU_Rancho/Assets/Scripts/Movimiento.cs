using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 8f;

    [Header("Slide")]
    public float slideSpeed = 10f;
    public float slideDuration = 0.4f;
    private bool isSliding = false;

    [Header("Detección de suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isGrounded;
    private float moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Movimiento horizontal
        moveInput = Input.GetAxisRaw("Horizontal");

        // --------------------------------------------------------------------
        // ANIMACIONES BÁSICAS
        // --------------------------------------------------------------------
        animator.SetBool("Walking", moveInput != 0 && !isSliding);
        animator.SetBool("Run", Input.GetKey(KeyCode.LeftShift) && moveInput != 0 && !isSliding);

        // --------------------------------------------------------------------
        // SALTO
        // --------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }

        // --------------------------------------------------------------------
        // ATAQUE
        // --------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("Atack");
        }

        // --------------------------------------------------------------------
        // PICK (recoger)
        // --------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("Pick");
        }

        // --------------------------------------------------------------------
        // SLIDE (Shift + S o Shift + W)
        // --------------------------------------------------------------------
        if (!isSliding && Input.GetKey(KeyCode.LeftShift) && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W)))
        {
            StartCoroutine(DoSlide());
        }
    }

    private void FixedUpdate()
    {
        // Detectar suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Si está deslizando, no mover normal
        if (isSliding)
            return;

        // Movimiento normal / correr
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Flip del personaje
        if (moveInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
    }

    // --------------------------------------------------------------------
    // SLIDE
    // --------------------------------------------------------------------
    private System.Collections.IEnumerator DoSlide()
    {
        isSliding = true;
        animator.SetBool("Slide", true);

        // Aplicamos impulso dependiendo de la dirección
        float slideDirection = transform.localScale.x;
        rb.linearVelocity = new Vector2(slideDirection * slideSpeed, rb.linearVelocity.y);

        yield return new WaitForSeconds(slideDuration);

        isSliding = false;
        animator.SetBool("Slide", false);
    }

    // Gizmo del suelo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}