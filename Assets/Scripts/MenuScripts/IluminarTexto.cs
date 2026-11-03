using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class IlumninarTexto : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text texto;          // El TMP_Text del botón
    public float minTime = 0.05f;   // Tiempo mínimo entre apagado/encendido
    public float maxTime = 0.2f;    // Tiempo máximo entre apagado/encendido

    private Coroutine parpadeoCoroutine;

    void Awake()
    {
        if (texto == null)
            texto = GetComponentInChildren<TMP_Text>();
    }

    // Cuando el ratón entra
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (parpadeoCoroutine == null)
            parpadeoCoroutine = StartCoroutine(Parpadeo());
    }

    // Cuando el ratón sale
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

    // Coroutine que apaga y enciende el texto aleatoriamente
    private IEnumerator Parpadeo()
    {
        while (true)
        {
            texto.enabled = !texto.enabled; // alterna encendido/apagado
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }
    }
}
