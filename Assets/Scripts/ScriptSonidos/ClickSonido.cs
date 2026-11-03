using UnityEngine;

/// <summary>Reproduce un sonido al pulsar E dentro del área del objeto.</summary>
public class ClickSonido : MonoBehaviour {
    /// <summary>Audio que se reproduce al interactuar.</summary>
    public AudioSource audioSource;

    /// <summary>Indica si el jugador está dentro del área de interacción.</summary>
    private bool enZona;

    /// <summary>Reproduce el sonido si el jugador pulsa E dentro de la zona.</summary>
    void Update() {
        // Permite interacción solo si el jugador está dentro del trigger y pulsa la tecla de acción (E)
        if (enZona && Input.GetKeyDown(KeyCode.E)) {
            // Ejecuta el sonido asociado al objeto
            audioSource.Play();
        }
    }

    /// <summary>
    /// Detecta la entrada del jugador en el área de interacción.
    /// </summary>
    /// <param name="other">Collider que entra en el trigger.</param>
    void OnTriggerEnter(Collider other) {
        // Detecta entrada del jugador en el área de interacción
        if (other.CompareTag("Player")) {
            enZona = true;
        }
    }

    /// <summary>
    /// Detecta la salida del jugador del área de interacción.
    /// </summary>
    /// <param name="other">Collider que sale del trigger.</param>
    void OnTriggerExit(Collider other) {
        // Detecta salida del jugador del área de interacción
        if (other.CompareTag("Player")) {
            enZona = false;
        }
    }
}