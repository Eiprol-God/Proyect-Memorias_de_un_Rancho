using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 25f;
    public Transform groundCheck;      // Tu GroundCheck asignado en el inspector
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;      // La capa del suelo

    private Rigidbody2D rb;
    private Animator anim;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // --- Revisión de suelo ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // --- Activar/desactivar animación de salto ---
        anim.SetBool("Jump", !isGrounded);

        // --- Salto ---
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // Para ver el GroundCheck en editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}