using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public float damage = 20f; // Daño que hace el arma al jefe
    public bool isAttacking = false; // Para saber si el jugador está atacando

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto con el que colisionamos es el jefe
        if (other.CompareTag("Boss") && isAttacking)
        {
            // Llamar al método que aplica el daño al jefe
            BossController bossController = other.GetComponent<BossController>();
            if (bossController != null)
            {
                bossController.TakeDamage(damage); // Pasar el daño al jefe
            }
        }
    }

    // Método para activar el ataque del arma
    public void StartAttack()
    {
        isAttacking = true;
    }

    // Método para desactivar el ataque del arma
    public void EndAttack()
    {
        isAttacking = false;
    }
}
