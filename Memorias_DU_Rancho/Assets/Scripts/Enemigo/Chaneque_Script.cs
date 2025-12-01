using UnityEngine;

public class EnemyAI2D : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;

    [Header("Detection")]
    public Transform wallCheck;
    public float wallCheckDistance = 0.2f;
    public LayerMask obstacleLayer;
    public float detectionRange = 5f;

    [Header("Combat")]
    public int maxHealth = 3; // NUEVO: Vida máxima del enemigo
    private int currentHealth; // NUEVO: Vida actual del enemigo
    public int damageToPlayer = 1;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform player;

    private bool isChasing = false;
    private bool isDead = false;
    private int direction = 1; // 1 para derecha, -1 para izquierda

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        FindPlayer();
        currentHealth = maxHealth; // NUEVO: Inicializar vida actual
    }

    private void Update()
    {
        if (isDead) return;
        if (player == null) return;

        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        DetectWall();
    }

    private void Patrol()
    {
        if (anim != null) anim.SetBool("isRunning", false);
        if (rb != null) rb.linearVelocity = new Vector2(walkSpeed * direction, rb.linearVelocity.y);
    }

    private void ChasePlayer()
    {
        if (anim != null) anim.SetBool("isRunning", true);

        if (player.position.x > transform.position.x)
            direction = 1;
        else
            direction = -1;

        Flip();

        if (rb != null) rb.linearVelocity = new Vector2(runSpeed * direction, rb.linearVelocity.y);
    }

    private void DetectWall()
    {
        if (wallCheck == null) return;

        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, Vector2.right * direction, wallCheckDistance, obstacleLayer);

        if (hit.collider != null)
        {
            direction *= -1;
            Flip();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.collider.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(damageToPlayer);
            }
        }
    }

    // NUEVO: Método para que el enemigo reciba daño
    public void TakeDamage(int damage)
    {
        if (isDead) return; // Si ya está muerto, no recibe más daño

        currentHealth -= damage;
        Debug.Log("Enemigo recibe daño. Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        // Opcional: Aquí podrías añadir efectos de sonido o visuales de daño
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    public void Die()
    {
        if (isDead) return; // Prevenir múltiples llamadas a Die
        isDead = true;
        Debug.Log("Enemigo ha muerto!");

        if (anim != null) anim.SetBool("isDead", true);
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }

        if(TryGetComponent<Collider2D>(out Collider2D col))
        {
            col.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
}