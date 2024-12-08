using UnityEngine;
using UnityEngine.AI;

public class MovableObstacle : MonoBehaviour
{
    public float radius = 5f;  // Radio del c�rculo
    public float moveInterval = 6f;  // Tiempo entre movimientos completos (circular)
    public float speed = 2f;  // Velocidad de movimiento
    public float warningDuration = 1f;  // Duraci�n de la advertencia
    public float warningInterval = 0.2f; // Intervalo entre advertencias
    public LayerMask detectionLayers;
    public Material warningMaterial;
    public Material normalMaterial;

    private float timer;
    private bool isMoving;
    private bool isWarningActive;
    private bool isInWarningPeriod; // Para saber si estamos en el periodo de advertencia
    private Renderer obstacleRenderer;
    private Vector3 centerPosition;  // El centro de rotaci�n
    private float angle;  // �ngulo de rotaci�n
    private float warningTimer;  // Temporizador para la advertencia
    private NavMeshObstacle navMeshObstacle;

    void Start()
    {
        // Inicializaci�n de componentes
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        obstacleRenderer = GetComponent<Renderer>();
        centerPosition = transform.position;  // El centro de rotaci�n es la posici�n inicial del obst�culo
        timer = moveInterval;
        warningTimer = warningInterval;
        navMeshObstacle.carving = true;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // Si estamos en el periodo de advertencia
        if (isInWarningPeriod)
        {
            warningTimer -= Time.deltaTime;

            // Alternar entre advertencia activa e inactiva
            if (warningTimer <= 0f)
            {
                isWarningActive = !isWarningActive;
                warningTimer = warningInterval;
            }

            // Mostrar advertencia si est� activa
            if (isWarningActive)
            {
                obstacleRenderer.material = warningMaterial;
            }
            else
            {
                obstacleRenderer.material = normalMaterial;
            }
        }
        else
        {
            // Si no estamos en advertencia, realizamos el movimiento circular
            if (timer <= 0 && !isMoving)
            {
                isInWarningPeriod = true;  // Iniciar el periodo de advertencia
                timer = moveInterval;
                isMoving = false; // No mover hasta despu�s de la advertencia
            }

            // Movimiento circular
            if (!isInWarningPeriod)
            {
                angle += speed * Time.deltaTime;  // Incrementar el �ngulo seg�n la velocidad
                if (angle > 360f) angle -= 360f;  // Asegurarse de que el �ngulo no se desborde

                // Calcular nueva posici�n en el c�rculo
                float x = centerPosition.x + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
                float z = centerPosition.z + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

                transform.position = new Vector3(x, transform.position.y, z);  // Mantener la altura actual (y)

                // Comprobar colisiones con enemigos o jugadores
                Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation, detectionLayers);
                foreach (Collider hit in hitColliders)
                {
                    if (hit.CompareTag("Player") || hit.CompareTag("Enemy"))
                    {
                        Debug.Log(hit.name + " fue impactado por el obst�culo.");
                    }
                }
            }
        }

        // Cuando termine el periodo de advertencia, se reanuda el movimiento
        if (isInWarningPeriod && timer <= warningDuration)
        {
            isInWarningPeriod = false;
        }
    }

    private void OnDrawGizmos()
    {
        // Gizmos para visualizar el movimiento circular
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centerPosition, radius);  // Dibuja el c�rculo
    }
}
