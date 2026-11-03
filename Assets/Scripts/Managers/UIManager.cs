using UnityEngine;

/// <summary>Conecta el sistema de preguntas con la interfaz de diálogo.</summary>
public class UIManager : MonoBehaviour {
    /// <summary>Sistema de diálogo encargado de mostrar las preguntas.</summary>
    public DialogoUI dialogoUI;

    /// <summary>Script que gestiona la carga y selección de preguntas.</summary>
    public PreguntasManager preguntasManager;


    /// <summary>
    /// Obtiene una pregunta aleatoria y abre el diálogo para mostrarla. Registra un error en consola si falta alguna referencia o no hay preguntas cargadas.
    /// </summary>
    public void MostrarPregunta() {

        // Comprueba que el sistema de preguntas esté disponible
        if (preguntasManager == null) {
            Debug.LogError("PreguntasManager no asignado en UIManager");
            return;
        }

        // Comprueba que exista una interfaz para mostrar el diálogo
        if (dialogoUI == null) {
            Debug.LogError("DialogoUI no asignado en UIManager");
            return;
        }

        // Obtiene una pregunta del conjunto actualmente cargado
        var pregunta = preguntasManager.GetPreguntaRandom();

        // Evita abrir la interfaz si no existen preguntas disponibles
        if (pregunta == null) {
            Debug.LogError("No hay preguntas cargadas (JSON no inicializado)");
            return;
        }

        // Abre el diálogo enviando pregunta y respuesta correcta
        dialogoUI.Abrir(
            pregunta.pregunta,
            pregunta.respuesta
        );
    }
}