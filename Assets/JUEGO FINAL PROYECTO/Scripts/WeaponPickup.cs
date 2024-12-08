using UnityEngine;
using TMPro; // Para usar TextMeshPro

public class WeaponPickup : MonoBehaviour
{
    public Transform playerHand; // Donde el arma se posicionará en la mano del jugador
    private bool isNear = false; // Para verificar si el jugador está cerca
    private GameObject player; // El objeto del jugador
    public TextMeshProUGUI pickupText; // Texto que aparece cuando el jugador está cerca del arma

    void Start()
    {
        player = GameObject.FindWithTag("Player"); // Suponiendo que el jugador tiene la etiqueta "Player"
        pickupText.gameObject.SetActive(false); // Al inicio, no mostrar el texto
    }

    void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.E)) // Si el jugador presiona "E"
        {
            PickUpWeapon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Si el jugador está cerca
        {
            isNear = true;
            pickupText.gameObject.SetActive(true); // Mostrar el texto
            pickupText.text = "Presiona E para coger el arma"; // Actualizar el texto
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Si el jugador se aleja
        {
            isNear = false;
            pickupText.gameObject.SetActive(false); // Ocultar el texto
        }
    }

    void PickUpWeapon()
    {
        // Desactivar el arma en el suelo temporalmente (pero no destruirla)
        this.gameObject.SetActive(false);

        // Colocar el arma en la mano del jugador (a la misma posición que la mano)
        this.transform.position = playerHand.position;
        this.transform.rotation = playerHand.rotation;

        // Hacer que el arma sea un hijo del jugador (y se mueva con la mano)
        this.transform.SetParent(playerHand);

        // Asegurarse de que el arma sea visible y activa después de ser recogida
        this.gameObject.SetActive(true);

        // Ocultar el texto después de que el arma se recoja
        pickupText.gameObject.SetActive(false);
    }
}
