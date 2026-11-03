using UnityEngine;

/// <summary>Objeto con el que el jugador interactúa para responder una pregunta de su prueba.</summary>
public class Interactuable : MonoBehaviour {
    /// <summary>Índice del banco de preguntas que pertenece a este objeto.</summary>
    public int indicePrueba = 0;

    /// <summary>Sistema que carga y proporciona las preguntas.</summary>
    public PreguntasManager preguntasManager;

    /// <summary>Interfaz usada para mostrar la pregunta y recoger la respuesta.</summary>
    public DialogoUI dialogoUI;

    /// <summary>Sistema que muestra el progreso mediante luces.</summary>
    public LucesPruebasManager lucesPruebasManager;


    /// <summary>
    /// Inicia la interacción: carga las preguntas de la prueba, elige una al azar y abre el diálogo.
    /// </summary>
    public void Interactuar() {

        // Evita abrir varios diálogos simultáneamente
        if (DialogoUI.EstaAbierto) {
            return;
        }

        // Guarda la prueba actual para acceso global desde GameManager
        GameManager.Instance.indicePruebaActual = indicePrueba;

        // Comprueba que el sistema de preguntas exista
        if (preguntasManager == null) {
            Debug.LogError(
                "PreguntasManager NO asignado en " + gameObject.name
            );

            return;
        }

        // Comprueba que exista una interfaz de diálogo
        if (dialogoUI == null) {
            Debug.LogError(
                "DialogoUI NO asignado en " + gameObject.name
            );

            return;
        }

        // Carga el conjunto de preguntas correspondiente a esta prueba
        preguntasManager.CargarPorIndice(indicePrueba);

        // Obtiene una pregunta aleatoria del conjunto cargado
        Pregunta p = preguntasManager.GetPreguntaRandom();

        // Cancela el flujo si no existen preguntas válidas
        if (p == null) {
            Debug.LogError("No hay preguntas cargadas");
            return;
        }

        // Abre el diálogo enviando pregunta, respuesta y el callback para respuesta correcta
        dialogoUI.Abrir(
            p.pregunta,
            p.respuesta,
            OnRespuestaCorrecta
        );
    }


    /// <summary>Se ejecuta cuando el jugador responde bien: enciende la luz de la prueba completada.</summary>
    private void OnRespuestaCorrecta() {

        // Busca el sistema de luces si no fue asignado manualmente
        if (lucesPruebasManager == null) {
            lucesPruebasManager =
                FindFirstObjectByType<LucesPruebasManager>();
        }

        // Comprueba que el sistema exista antes de continuar
        if (lucesPruebasManager == null) {
            Debug.LogError(
                "LucesPruebasManager NO asignado en "
                + gameObject.name
            );

            return;
        }

        // Activa la luz asociada a la prueba completada
        lucesPruebasManager.ActivarLuz(
            indicePrueba + 1
        );
    }
}