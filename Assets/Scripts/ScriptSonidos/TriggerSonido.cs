using UnityEngine;

public class TriggerSonido : MonoBehaviour
{
    // Audio que se reproducirá al entrar en el trigger
    public AudioSource sonido;

    // Evita que el sonido se dispare múltiples veces
    private bool activado = false;

    private void OnTriggerEnter(Collider other) {
        // Detecta colisión con el jugador y asegura ejecución única
        if (other.CompareTag("Player") && !activado) {
            // Bloquea futuras activaciones del trigger
            activado = true;

            // Reproduce el sonido asociado al evento
            sonido.Play();
        }
    }
}