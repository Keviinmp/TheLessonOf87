using UnityEngine;
using System.Collections;
using TMPro;

public class TriggerVictoria : MonoBehaviour
{
    [Header("Canvas Victoria")]
    public GameObject canvasVictoria;
    public CanvasGroup canvasGroup;

    [Header("UI Tiempo")]
    public TMP_Text textoTiempo;

    [Header("Audio")]
    public AudioSource audioVictoria;

    [Header("Fade")]
    public float fadeSpeed = 1f;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado) return;

        if (other.CompareTag("Player"))
        {
            activado = true;
            StartCoroutine(MostrarVictoria());
        }
    }

    IEnumerator MostrarVictoria()
    {
        // ACTIVAR CANVAS
        canvasVictoria.SetActive(true);

        // asegurar orden UI
        canvasVictoria.transform.SetAsLastSibling();

        // SONIDO
        if (audioVictoria != null)
            audioVictoria.Play();

        // 🔥 MOSTRAR TIEMPO DEL JUEGO
        if (GameManager.Instance != null && textoTiempo != null)
        {
            textoTiempo.text = "TIEMPO: " + GameManager.Instance.ObtenerTiempoFormateado();
        }

        // FADE IN
        float t = 0f;
        canvasGroup.alpha = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * fadeSpeed;
            canvasGroup.alpha = t;
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // desbloquear cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}