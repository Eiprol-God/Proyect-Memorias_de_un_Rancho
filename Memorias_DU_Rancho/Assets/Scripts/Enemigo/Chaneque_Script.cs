using UnityEngine;

public class EnemyAI2D : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;

    [Header("Detection")]
    public Transform wallCheck;
    public Transform groundCheck; // Nueva variable para detectar el suelo
    public float wallCheckDistance = 0.2f;
    public float groundCheckDistance = 0.4f; // Nueva variable
    public LayerMask obstacleLayer;
    public float detectionRange = 5f;

    [Header("Combat")]
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

        // Intentar encontrar al jugador al inicio
        FindPlayer();
    }

    private void Update()
    {
        if (isDead) return;

        // Si no se encontró al jugador, no hacer nada.
        if (player == null)
        {
            return;
        }

        // Decidir si patrullar o perseguir
        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        // Ejecutar estado
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        // Comprobar si hay muros
        DetectWall();
    }

    private void Patrol()
    {
        if (anim != null) anim.SetBool("isRunning", false);

        // --- NUEVA LÓGICA DE DETECCIÓN DE SUELO ---
        if (groundCheck != null)
        {
            // Lanza un rayo hacia abajo para ver si hay suelo
            RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, obstacleLayer);
            if (groundInfo.collider == null)
            {
                // Si no hay suelo enfrente, darse la vuelta
                if (direction == 1)
                {
                    direction = -1;
                }
                else
                {
                    direction = 1;
                }
                Flip();
            }
        }
        // --- FIN DE LA NUEVA LÓGICA ---

        if (rb != null) rb.linearVelocity = new Vector2(walkSpeed * direction, rb.linearVelocity.y);
    }

    private void ChasePlayer()
    {
        if (anim != null) anim.SetBool("isRunning", true);

        // Cambiar dirección hacia el jugador
        if (player.position.x > transform.position.x)
            direction = 1;
        else
            direction = -1;

        Flip();

        if (rb != null) rb.linearVelocity = new Vector2(runSpeed * direction, rb.linearVelocity.y);
    }

    private void DetectWall()
    {
        // Salir si la referencia de wallCheck no está asignada en el Inspector
        if (wallCheck == null) return;

        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, Vector2.right * direction, wallCheckDistance, obstacleLayer);

        if (hit.collider != null)
        {
            direction *= -1; // Cambiar dirección
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
            // Usar TryGetComponent para más seguridad
            if (collision.collider.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(damageToPlayer);
            }
        }
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
        isDead = true;
        if (anim != null) anim.SetBool("isDead", true);
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true; // Evita que siga siendo afectado por la física al morir
        }
        
        if(TryGetComponent<Collider2D>(out Collider2D col))
        {
            col.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
}
