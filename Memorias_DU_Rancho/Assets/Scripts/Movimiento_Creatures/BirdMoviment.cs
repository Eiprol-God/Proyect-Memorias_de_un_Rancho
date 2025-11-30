using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float minSpeed = 1.5f;
    public float maxSpeed = 3.5f;
    public float startX = -7.93f;
    public float endX = 253.5f;

    [Header("Altura aleatoria")]
    public float minY = -1f;
    public float maxY = 3f;

    [Header("Rotación (bamboleo)")]
    public float rotationAmount = 10f;      // Grados que inclina el ave
    public float rotationSpeed = 2f;        // Qué tan rápido inclina

    [Header("Animación")]
    public Animator animator;

    private float speed;
    private float randomOffset;

    void Start()
    {
        // Velocidad aleatoria
        speed = Random.Range(minSpeed, maxSpeed);

        // Posición inicial aleatoria en Y
        float randomY = Random.Range(minY, maxY);
        transform.position = new Vector3(startX, randomY, transform.position.z);

        // Desfase para que no roten sincronizados
        randomOffset = Random.Range(0f, 100f);

        // Animación
        if (animator != null)
            animator.Play("Flutter");
    }

    void Update()
    {
        // Movimiento horizontal
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Rotación suave (como si se balanceara en el aire)
        float rot = Mathf.Sin((Time.time + randomOffset) * rotationSpeed) * rotationAmount;
        transform.rotation = Quaternion.Euler(0, 0, rot);

        // Reiniciar posición
        if (transform.position.x >= endX)
        {
            float newY = Random.Range(minY, maxY);
            transform.position = new Vector3(startX, newY, transform.position.z);
        }
    }
}