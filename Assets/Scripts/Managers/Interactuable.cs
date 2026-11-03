using UnityEngine;

public class Interactuable : MonoBehaviour
{
    // Índice que identifica qué prueba/puzzle pertenece a este objeto interactuable
    public int indicePrueba = 0;

    // Sistema de gestión de preguntas del juego
    public PreguntasManager preguntasManager;

    // UI de diálogo donde se muestra la pregunta al jugador
    public DialogoUI dialogoUI;

    // Sistema de feedback visual (luces, estados de pruebas, etc.)
    public LucesPruebasManager lucesPruebasManager;

    // Método principal de interacción llamado desde el sistema de input/raycast
    // Encargado de iniciar el flujo de pregunta → respuesta → feedback
    public void Interactuar()
    {
        // Evita interacción simultánea si ya hay un diálogo abierto
        if (DialogoUI.EstaAbierto)
            return;

        // Guarda el índice de la prueba actual en el GameManager global
        GameManager.Instance.indicePruebaActual = indicePrueba;

        // Validación del sistema de preguntas
        if (preguntasManager == null)
        {
            Debug.LogError("PreguntasManager NO asignado en " + gameObject.name);
            return;
        }

        // Validación del sistema de UI
        if (dialogoUI == null)
        {
            Debug.LogError("DialogoUI NO asignado en " + gameObject.name);
            return;
        }

        // Carga el conjunto de preguntas asociado a esta prueba
        preguntasManager.CargarPorIndice(indicePrueba);

        // Obtiene una pregunta aleatoria del pool cargado
        Pregunta p = preguntasManager.GetPreguntaRandom();

        // Evita continuar si no hay datos válidos
        if (p == null)
        {
            Debug.LogError("No hay preguntas cargadas");
            return;
        }

        // Abre el diálogo de UI y define callback de respuesta correcta
        dialogoUI.Abrir(p.pregunta, p.respuesta, OnRespuestaCorrecta);
    }

    // Método que se ejecuta cuando el jugador responde bien
    // Activa el progreso de la prueba en el sistema de luces
    private void OnRespuestaCorrecta()
    {
        // Si no está asignado, intenta resolverlo
        if (lucesPruebasManager == null)
            lucesPruebasManager = FindFirstObjectByType<LucesPruebasManager>();

        // Validación final del sistema de feedback visual
        if (lucesPruebasManager == null)
        {
            Debug.LogError("LucesPruebasManager NO asignado en " + gameObject.name);
            return;
        }

        // Activa el siguiente estado visual asociado a la prueba superada
        lucesPruebasManager.ActivarLuz(indicePrueba + 1);
    }
}