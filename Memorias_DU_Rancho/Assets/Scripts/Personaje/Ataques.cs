using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;

    [Header("Configuración de Ataque")]
    public Transform attackPoint; // El punto desde donde se lanza el ataque.
    public float attackRange = 0.5f; // El rango del ataque.
    public LayerMask enemyLayer; // La capa en la que se encuentran los enemigos.
    public int attackDamage = 1; // El daño del ataque.

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleAttackInput();
    }

    void HandleAttackInput()
    {
        // Attack1 → X
        if (Input.GetKeyDown(KeyCode.X))
        {
            // Activar la animación de ataque
            if (anim != null)
            {
                anim.SetTrigger("Attack1");
            }

            // --- NUEVA LÓGICA PARA DETECTAR Y DAÑAR ENEMIGOS ---
            
            // 1. Validar que el attackPoint ha sido asignado en el Inspector
            if (attackPoint == null)
            {
                Debug.LogError("¡Falta asignar el AttackPoint en el Inspector!");
                return;
            }

            // 2. Detectar todos los enemigos dentro del círculo de ataque
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

            // 3. Aplicar daño a cada enemigo encontrado
            foreach(Collider2D enemy in hitEnemies)
            {
                Debug.Log("Golpeamos a: " + enemy.name);
                // Intentar obtener el script del enemigo y aplicar daño
                if (enemy.TryGetComponent<EnemyAI2D>(out EnemyAI2D enemyScript))
                {
                    enemyScript.TakeDamage(attackDamage);
                }
            }
        }
    }

    // Opcional: Dibujar el rango de ataque en el editor para visualizarlo
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
