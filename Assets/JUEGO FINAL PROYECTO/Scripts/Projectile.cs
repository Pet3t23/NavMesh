using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f; // Asegúrate de que la velocidad sea alta
    public float lifetime = 5f; // Aumenta este valor para permitir que las balas viajen más lejos

    void Start()
    {
        Destroy(gameObject, lifetime); // Destruye el proyectil después de 'lifetime' segundos
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime; // Movimiento continuo
    }
}
