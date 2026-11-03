using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// Muestra la pantalla de victoria, con el tiempo total y un fundido, cuando el jugador entra en el trigger final.
/// </summary>
public class TriggerVictoria : MonoBehaviour {

    /// <summary>Panel de victoria que se activa al terminar el juego.</summary>
    [Header("Canvas Victoria")]

    public GameObject canvasVictoria;

    /// <summary>CanvasGroup del panel, usado para el fundido.</summary>
    public CanvasGroup canvasGroup;

    /// <summary>Texto donde se muestra el tiempo total de la partida.</summary>
    [Header("UI Tiempo")]

    public TMP_Text textoTiempo;

    /// <summary>Audio que suena al mostrar la pantalla de victoria.</summary>
    [Header("Audio")]

    public AudioSource audioVictoria;

    /// <summary>Velocidad del fundido de entrada.</summary>
    [Header("Fade")]

    public float fadeSpeed = 1f;

    /// <summary>Evita activar el trigger varias veces.</summary>
    private bool activado = false;


    /// <summary>
    /// Lanza la secuencia de victoria cuando entra el jugador.
    /// </summary>
    /// <param name="other">Collider que entra en el trigger.</param>
    private void OnTriggerEnter(Collider other) {

        if (activado) return;

        if (other.CompareTag("Player")) {
            activado = true;
            StartCoroutine(MostrarVictoria());
        }
    }


    /// <summary>
    /// Secuencia de victoria: muestra el panel, reproduce el audio, escribe el tiempo, hace el fundido y libera el cursor.
    /// </summary>
    IEnumerator MostrarVictoria() {

        // Activa el canvas de la victoria
        canvasVictoria.SetActive(true);

        // Asegura que el canvas esté por encima del resto de UI
        canvasVictoria.transform.SetAsLastSibling();

        // Reproduce el sonido de victoria si está asignado
        if (audioVictoria != null)
            audioVictoria.Play();

        // Muestra el tiempo total de partida desde el GameManager
        if (GameManager.Instance != null && textoTiempo != null) {
            textoTiempo.text =
                "TIEMPO: " + GameManager.Instance.ObtenerTiempoFormateado();
        }

        // Fade in del canvas
        float t = 0f;
        canvasGroup.alpha = 0f;

        while (t < 1f) {
            t += Time.unscaledDeltaTime * fadeSpeed;
            canvasGroup.alpha = t;
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // Libera el cursor para interacción en menú final
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}