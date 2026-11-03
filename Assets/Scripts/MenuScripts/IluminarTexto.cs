using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>Hace parpadear el texto de un botón mientras el ratón está encima.</summary>
public class IlumninarTexto : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>Texto del botón que parpadea.</summary>
    public TMP_Text texto;
    /// <summary>Tiempo mínimo entre apagado y encendido, en segundos.</summary>
    public float minTime = 0.05f;
    /// <summary>Tiempo máximo entre apagado y encendido, en segundos.</summary>
    public float maxTime = 0.2f;

    /// <summary>Corrutina de parpadeo en ejecución, o <c>null</c> si no hay ninguna.</summary>
    private Coroutine parpadeoCoroutine;

    /// <summary>Busca el componente de texto en los hijos si no se asignó manualmente.</summary>
    void Awake()
    {
        if (texto == null)
            texto = GetComponentInChildren<TMP_Text>();
    }

    /// <summary>
    /// Inicia el parpadeo cuando el ratón entra en el botón.
    /// </summary>
    /// <param name="eventData">Datos del evento de puntero.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (parpadeoCoroutine == null)
            parpadeoCoroutine = StartCoroutine(Parpadeo());
    }

    /// <summary>
    /// Detiene el parpadeo cuando el ratón sale y deja el texto encendido.
    /// </summary>
    /// <param name="eventData">Datos del evento de puntero.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (parpadeoCoroutine != null)
        {
            StopCoroutine(parpadeoCoroutine);
            parpadeoCoroutine = null;
        }

        // Asegurarse de que el texto quede encendido
        texto.enabled = true;
    }

    /// <summary>Corrutina que enciende y apaga el texto en intervalos aleatorios.</summary>
    private IEnumerator Parpadeo()
    {
        while (true)
        {
            texto.enabled = !texto.enabled; // alterna encendido/apagado
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }
    }
}
