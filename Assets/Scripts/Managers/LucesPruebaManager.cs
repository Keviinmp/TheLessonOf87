using UnityEngine;
using System.Collections;

/// <summary>
/// Muestra el progreso del jugador con cuatro luces y lanza el evento final al completar las cuatro pruebas.
/// </summary>
public class LucesPruebasManager : MonoBehaviour  {
    /// <summary>Animador de la luz de la prueba 1.</summary>
    public Animator luz1;
    /// <summary>Animador de la luz de la prueba 2.</summary>
    public Animator luz2;
    /// <summary>Animador de la luz de la prueba 3.</summary>
    public Animator luz3;
    /// <summary>Animador de la luz de la prueba 4.</summary>
    public Animator luz4;

    /// <summary>Objeto que desaparece al completar todas las pruebas.</summary>
    [Header("Evento final")]

    public GameObject objetoFinal;

    /// <summary>Luces exteriores que se encienden durante el evento final.</summary>
    public Light[] lucesFuera;

    /// <summary>Segundos de espera antes de lanzar el evento final.</summary>
    [Header("Tiempo tras última luz")]

    public float esperaEventoFinal = 2f;

    /// <summary>Número de pruebas completadas.</summary>
    private int lucesActivadas = 0;

    /// <summary>Evita lanzar el evento final más de una vez.</summary>
    private bool eventoLanzado = false;


    /// <summary>Apaga las luces exteriores al iniciar.</summary>
    private void Start() {
        if (lucesFuera != null) {
            foreach (Light l in lucesFuera) {
                if (l != null) {
                    l.enabled = false;
                }
            }
        }
    }


    /// <summary>
    /// Enciende la luz de una prueba completada y lanza el evento final cuando ya están las cuatro.
    /// </summary>
    /// <param name="numeroPrueba">Número de la prueba completada (de 1 a 4).</param>
    public void ActivarLuz(int numeroPrueba) {
        if (eventoLanzado)
            return;

        Animator luz = GetLuz(numeroPrueba);

        if (luz == null) {
            Debug.LogError("No hay Animator asignado para la luz " + numeroPrueba);
            return;
        }

        // Ejecuta la animación de encendido de luces
        luz.SetTrigger("Activar");

        // Actualiza el progreso total de las luces que hay encendidas
        lucesActivadas++;

        Debug.Log("Luz activada: " + numeroPrueba + " | Total: " + lucesActivadas);

        // Al completar todas las pruebas (luces encendidas = 4) inicia la secuencia final
        if (lucesActivadas >= 4) {
            eventoLanzado = true;

            StartCoroutine(EventoFinal());
        }
    }


    /// <summary>Tras una espera, oculta el objeto final y enciende las luces exteriores.</summary>
    private IEnumerator EventoFinal() {
        // Espera antes de aplicar el cambio de estado
        yield return new WaitForSeconds(esperaEventoFinal);

        Debug.Log("EVENTO FINAL");

        // Oculta el objeto principal del evento
        if (objetoFinal != null) {
            objetoFinal.SetActive(false);
        }

        // Activa iluminación exterior
        if (lucesFuera != null) {
            foreach (Light l in lucesFuera) {
                if (l != null) {
                    l.enabled = true;
                }
            }
        }
    }


    /// <summary>
    /// Devuelve el animador de la luz de una prueba.
    /// </summary>
    /// <param name="numeroPrueba">Número de la prueba (de 1 a 4).</param>
    /// <returns>El <c>Animator</c> correspondiente, o <c>null</c> si el número no es válido.</returns>
    private Animator GetLuz(int numeroPrueba) {
        switch (numeroPrueba) {
            case 1:
                return luz1;

            case 2:
                return luz2;

            case 3:
                return luz3;

            case 4:
                return luz4;

            default:
                Debug.LogError("Numero invalido");
                return null;
        }
    }
}