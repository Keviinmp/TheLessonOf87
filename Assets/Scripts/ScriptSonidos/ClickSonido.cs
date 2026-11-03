using UnityEngine;

public class ClickSonido : MonoBehaviour
{
    // Audio que se reproduce al interactuar
    public AudioSource audioSource;

    // Control de si el jugador está dentro del área de interacción
    private bool enZona;

    void Update() {
        // Permite interacción solo si el jugador está dentro del trigger y pulsa la tecla de acción (E)
        if (enZona && Input.GetKeyDown(KeyCode.E)) {
            // Ejecuta el sonido asociado al objeto
            audioSource.Play();
        }
    }

    // Método llamado automáticamente por Unity cuando otro el player entra en el trigger
    void OnTriggerEnter(Collider other) {
        // Detecta entrada del jugador en el área de interacción
        if (other.CompareTag("Player")) {
            enZona = true;
        }
    }

    // Método llamado automáticamente por Unity cuando otro el player sale del el trigger
    void OnTriggerExit(Collider other) {
        // Detecta salida del jugador del área de interacción
        if (other.CompareTag("Player")) {
            enZona = false;
        }
    }
}