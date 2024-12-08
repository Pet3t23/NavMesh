using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Vida máxima del jugador
    private float currentHealth;   // Vida actual del jugador

    public TextMeshProUGUI healthText; // Referencia al texto de vida en el UI

    void Start()
    {
        currentHealth = maxHealth; // Inicializar la vida al máximo
        UpdateHealthText();        // Actualizar el texto al inicio
    }

    // Método para recibir daño
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Reducir la vida actual
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Evitar que baje de 0

        UpdateHealthText(); // Actualizar la interfaz con la nueva vida

        if (currentHealth <= 0)
        {
            Die(); // Llamar a la función de muerte si la vida llega a 0
        }
    }

    // Método para actualizar el texto de la vida
    void UpdateHealthText()
    {
        healthText.text = "Vida: " + currentHealth.ToString("F0"); // Mostrar la vida sin decimales
    }

    // Método de muerte
    void Die()
    {
        Debug.Log("El jugador ha muerto.");
        // Aquí puedes agregar lógica adicional como cargar la pantalla de derrota
    }
}
