using UnityEngine;

/// <summary>Reproduce un sonido una sola vez cuando el jugador entra en el trigger.</summary>
public class TriggerSonido : MonoBehaviour {
    /// <summary>Audio que se reproduce al entrar en el trigger.</summary>
    public AudioSource sonido;

    /// <summary>Evita que el sonido se dispare más de una vez.</summary>
    private bool activado = false;

    /// <summary>
    /// Reproduce el sonido la primera vez que entra el jugador.
    /// </summary>
    /// <param name="other">Collider que entra en el trigger.</param>
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