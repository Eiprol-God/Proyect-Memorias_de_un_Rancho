using UnityEngine;

public class EnemyAI2D : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 4f;

    public Transform wallCheck;
    public float wallCheckDistance = 0.2f;
    public LayerMask obstacleLayer;

    public float detectionRange = 5f;
    public int damageToPlayer = 1;

    private Rigidbody2D rb;
    private Animator anim;

    private Transform player;
    private bool isChasing = false;
    private bool isDead = false;
    private int direction = 1; // 1 = derecha, -1 = izquierda

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isDead) return;

        float distanceFromPlayer = Vector2.Distance(transform.position, player.position);

        // ¿Jugador cerca?
        if (distanceFromPlayer <= detectionRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        // Movimiento
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        // Detectar muro
        DetectWall();
    }

    private void Patrol()
    {
        anim.SetBool("isRunning", false);

        rb.linearVelocity = new Vector2(walkSpeed * direction, rb.linearVelocity.y);
    }

    private void ChasePlayer()
    {
        anim.SetBool("isRunning", true);

        // Cambiar dirección hacia el jugador
        if (player.position.x > transform.position.x)
            direction = 1;
        else
            direction = -1;

        Flip(); // <-- Añade esta línea

        rb.linearVelocity = new Vector2(runSpeed * direction, rb.linearVelocity.y);
    }

    private void DetectWall()
    {
        // Raycast para detectar muro justo enfrente
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, Vector2.right * direction, wallCheckDistance, obstacleLayer);

        if (hit.collider != null)
        {
            direction *= -1; // Cambiar dirección
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Daño al jugador al tocarlo
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth ph = collision.collider.GetComponent<PlayerHealth>();

            if (ph != null)
            {
                ph.TakeDamage(damageToPlayer);
            }
        }
    }

    public void Die()
    {
        isDead = true;
        anim.SetBool("isDead", true);
        rb.linearVelocity = Vector2.zero; // Se queda quieto
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 2f);
    }
}
