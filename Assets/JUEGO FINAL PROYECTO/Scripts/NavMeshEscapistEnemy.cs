using UnityEngine;
using UnityEngine.AI;

public class NavMeshEscapistEnemy : MonoBehaviour
{
    public Transform player;
    public Transform fleePoint; // Punto de huida fijo
    public float detectionRadius = 10f;
    public float fleeDistance = 5f;
    public float activeDuration = 5f;
    public float tiredDuration = 3f;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootInterval = 2f;
    public LayerMask lineOfSightMask;

    private NavMeshAgent agent;
    private bool isTired = false;
    private float currentTiredTimer = 0f;
    private float currentActiveTimer = 0f;
    private float shootTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on " + gameObject.name);
        }
        if (player == null)
        {
            Debug.LogError("Player Transform not assigned.");
        }
        if (fleePoint == null)
        {
            Debug.LogError("Flee Point not assigned.");
        }
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (isTired)
        {
            HandleTiredState();
        }
        else
        {
            HandleActiveState();
        }

        if (CheckLineOfSight() && shootTimer >= shootInterval)
        {
            ShootAtPlayer();
            shootTimer = 0f;
        }
    }

    private void HandleTiredState()
    {
        currentTiredTimer += Time.deltaTime;
        agent.ResetPath(); // Deja de moverse

        // Representación visual de cansancio
        GetComponent<Renderer>().material.color = Color.blue;

        if (currentTiredTimer >= tiredDuration)
        {
            isTired = false;
            currentActiveTimer = 0f;
            GetComponent<Renderer>().material.color = Color.red; // Vuelve al color activo
        }
    }

    private void HandleActiveState()
    {
        currentActiveTimer += Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool hasLineOfSight = CheckLineOfSight(); // Evaluamos la línea de visión

        // Lógica de movimiento basada en línea de visión y distancia
        if (!hasLineOfSight && !isTired)
        {
            if (distanceToPlayer > detectionRadius)
            {
                agent.SetDestination(player.position); // Sigue al jugador si no está en rango
                Debug.Log("Enemigo siguiendo al jugador.");
            }
            else
            {
                // Cuando el jugador está cerca, huye al punto de huida
                agent.SetDestination(fleePoint.position);
                Debug.Log("Enemigo huyendo al punto de huida.");
            }
        }
        else if (hasLineOfSight)
        {
            agent.ResetPath(); // Detener movimiento si tiene línea de visión
            Debug.Log("Enemigo deteniéndose por línea de visión.");
        }

        // Cambia a estado cansado si llega al punto o al agotarse el tiempo activo
        if (agent.remainingDistance <= agent.stoppingDistance && agent.hasPath)
        {
            isTired = true;
            currentTiredTimer = 0f;
        }

        if (currentActiveTimer >= activeDuration)
        {
            isTired = true;
            currentTiredTimer = 0f;
        }
    }

    private bool CheckLineOfSight()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.position - transform.position;

        // Visualiza el rayo en la escena (opcional para debug)
        Debug.DrawRay(transform.position, directionToPlayer.normalized * 10000f, Color.red, 0.1f);

        // Raycast con distancia alta para simular visión infinita
        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, 10000f, lineOfSightMask))
        {
            if (hit.transform.gameObject == player.gameObject) // Comparación con el GameObject
            {
                Debug.Log("Raycast: Jugador detectado.");
                return true; // Línea de visión al jugador
            }
        }
        return false; // No hay línea de visión
    }

    private void ShootAtPlayer()
    {
        if (shootPoint != null && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (player.position - shootPoint.position).normalized;
                rb.velocity = direction * 10f; // Velocidad de la bala
            }
        }
    }
}
