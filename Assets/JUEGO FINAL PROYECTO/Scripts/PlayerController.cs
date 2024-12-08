using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public WeaponAttack weaponAttack; // Referencia al script de ataque del arma

    void Update()
    {
        // Comprobar si el jugador presiona el botón de ataque (clic izquierdo)
        if (Input.GetMouseButtonDown(0)) // 0 es el clic izquierdo
        {
            Attack();
        }

        if (Input.GetMouseButtonUp(0)) // Al soltar el clic izquierdo
        {
            EndAttack();
        }
    }

    void Attack()
    {
        // Iniciar el ataque del arma
        if (weaponAttack != null)
        {
            weaponAttack.StartAttack();
        }
    }

    void EndAttack()
    {
        // Terminar el ataque del arma
        if (weaponAttack != null)
        {
            weaponAttack.EndAttack();
        }
    }
}
