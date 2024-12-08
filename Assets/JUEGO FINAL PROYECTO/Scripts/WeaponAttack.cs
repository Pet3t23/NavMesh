using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public float damage = 20f; // Da�o que hace el arma al jefe
    public bool isAttacking = false; // Para saber si el jugador est� atacando

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto con el que colisionamos es el jefe
        if (other.CompareTag("Boss") && isAttacking)
        {
            // Llamar al m�todo que aplica el da�o al jefe
            BossController bossController = other.GetComponent<BossController>();
            if (bossController != null)
            {
                bossController.TakeDamage(damage); // Pasar el da�o al jefe
            }
        }
    }

    // M�todo para activar el ataque del arma
    public void StartAttack()
    {
        isAttacking = true;
    }

    // M�todo para desactivar el ataque del arma
    public void EndAttack()
    {
        isAttacking = false;
    }
}
