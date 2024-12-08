using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement; // Necesario para las escenas
using TMPro; // Necesario para TextMeshPro

public class BossController : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    private NavMeshAgent agent; // El agente para mover al jefe
    private Animator animator; // El animator para controlar las animaciones
    private bool isDead = false; // Para verificar si el jefe está muerto
    private bool isAttacking = false; // Para verificar si el jefe está atacando

    // Variables de salud del jefe
    public float health = 100f;
    public float attackRange = 2f;
    public float chaseRange = 10f;
    public float visionRange = 15f;
    public float attackDamage = 20f;
    public float maxHealth = 100f;
    private float currentHealth;

    private PlayerHealth playerHealth;
    private AudioSource audioSource;
    public AudioClip musicClip;

    public TextMeshProUGUI healthText;
    public string sceneToLoad = "WinScene"; // **Nuevo**: Escena a cargar cuando muera el jefe

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerHealth = player.GetComponent<PlayerHealth>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= visionRange)
        {
            if (distanceToPlayer <= chaseRange)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(musicClip);
                }

                healthText.enabled = true;

                if (distanceToPlayer > attackRange + 0.5f)
                {
                    ChasePlayer();
                }
                else if (distanceToPlayer <= attackRange)
                {
                    StopAndAttack();
                }
            }
        }
        else
        {
            StopChase();
        }

        UpdateHealthText();
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator.SetBool("isRunning", true);
    }

    private void StopAndAttack()
    {
        agent.isStopped = true;
        animator.SetBool("isRunning", false);

        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("isPunching");
            StartCoroutine(AttackCooldown());
        }
    }

    private void StopChase()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        healthText.enabled = false;
        agent.isStopped = true;
        animator.SetBool("isRunning", false);
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        agent.isStopped = true;
        animator.SetBool("isDead", true);
        healthText.enabled = false;

        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Esperar a que termine la animación de morir
        float deathAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(deathAnimationLength);

        // Cargar la escena después de la animación
        SceneManager.LoadScene(sceneToLoad);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    private void UpdateHealthText()
    {
        healthText.text = "Health: " + currentHealth.ToString("F0");
    }
}
