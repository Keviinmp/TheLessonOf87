using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class DialogoUI : MonoBehaviour
{
    // Esta bandera evita que el jugador vuelva a pulsar E mientras escribe la respuesta.
    public static bool EstaAbierto { get; private set; }

    public TMP_Text textoPregunta;
    public TMP_InputField inputRespuesta;

    public Camera camaraPuerta;
    public Camera camaraJugador;

    [Header("Transicion de camara")]
    public CanvasGroup panelTransicion;
    public float duracionFundido = 0.5f;
    public float tiempoCamaraPuerta = 3f;

    [Header("Estilo visual")]
    public bool aplicarEstiloAutomatico = true;
    public string textoBotonEnviar = "Responder";

    private string respuestaCorrecta;
    private Action alResponderCorrecto;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (aplicarEstiloAutomatico)
            AplicarEstiloVisual();

        PrepararPanelTransicion();
    }

    public void Abrir(string pregunta, string respuesta, Action alResponderCorrecto = null)
    {
        textoPregunta.text = pregunta;
        inputRespuesta.text = "";
        respuestaCorrecta = respuesta;
        this.alResponderCorrecto = alResponderCorrecto;

        gameObject.SetActive(true);
        MostrarDialogo(true);
        EstaAbierto = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    // Método para enviar la respuesta 
    public void Enviar()
{
    string respuestaJugador = inputRespuesta.text.Trim();

    if (respuestaJugador.ToLower() == respuestaCorrecta.ToLower())
    {
        Debug.Log("Correcto!");

        MostrarDialogo(false);
        StartCoroutine(MostrarCamaraPuertaYCerrar());
        return;
    }

    Debug.Log($"Incorrecto. La respuesta correcta era: {respuestaCorrecta}");

    // GUARDAR ERROR EN GAME MANAGER
    if (GameManager.Instance != null)
    {
        GameManager.Instance.RegistrarError(
            "Pregunta: " + textoPregunta.text +
            " | Correcta: " + respuestaCorrecta +
            " | Tú: " + respuestaJugador
        );
    }

    // PERDER VIDA
    GameManager.Instance.PerderVida();

    Cerrar();
}

    private void MostrarDialogo(bool visible)
    {
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }

    IEnumerator MostrarCamaraPuertaYCerrar()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        // Primero fundimos a negro, cambiamos la camara, y luego volvemos a mostrar la imagen.
        yield return Fundido(1f);

        if (camaraJugador != null)
            camaraJugador.enabled = false;

        if (camaraPuerta != null)
            camaraPuerta.enabled = true;

        alResponderCorrecto?.Invoke();

        yield return Fundido(0f);
        yield return new WaitForSecondsRealtime(tiempoCamaraPuerta);
        yield return Fundido(1f);

        if (camaraPuerta != null)
            camaraPuerta.enabled = false;

        if (camaraJugador != null)
            camaraJugador.enabled = true;

        yield return Fundido(0f);

        Cerrar();
    }

    private void AplicarEstiloVisual()
    {
        RectTransform panelRect = GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(820f, 430f);

        Image fondo = GetComponent<Image>();

        if (fondo == null)
            fondo = gameObject.AddComponent<Image>();

        fondo.color = new Color(0.035f, 0.04f, 0.045f, 0.96f);
        fondo.raycastTarget = true;

        // La pregunta queda arriba, grande y con margen, como pantalla de terminal.
        if (textoPregunta != null)
        {
            RectTransform preguntaRect = textoPregunta.GetComponent<RectTransform>();
            preguntaRect.anchorMin = new Vector2(0.5f, 1f);
            preguntaRect.anchorMax = new Vector2(0.5f, 1f);
            preguntaRect.pivot = new Vector2(0.5f, 1f);
            preguntaRect.anchoredPosition = new Vector2(0f, 100f);
            preguntaRect.sizeDelta = new Vector2(1200f, 150f);

            textoPregunta.color = new Color(0.86f, 0.96f, 0.88f, 1f);
            textoPregunta.fontSize = 110f;
            textoPregunta.enableAutoSizing = true;
            textoPregunta.fontSizeMin = 100f;
            textoPregunta.fontSizeMax = 142f;
            textoPregunta.alignment = TextAlignmentOptions.Center;
            textoPregunta.textWrappingMode = TextWrappingModes.Normal;
        }

        EstilizarInput();
        EstilizarBoton();
    }

    private void EstilizarInput()
    {
        if (inputRespuesta == null)
            return;

        RectTransform inputRect = inputRespuesta.GetComponent<RectTransform>();
        inputRect.anchorMin = new Vector2(0.5f, 0.5f);
        inputRect.anchorMax = new Vector2(0.5f, 0.5f);
        inputRect.pivot = new Vector2(0.5f, 0.5f);

        inputRect.anchoredPosition = new Vector2(0f, -10f);
        inputRect.sizeDelta = new Vector2(850f, 80f);

        Image inputFondo = inputRespuesta.GetComponent<Image>();

        if (inputFondo != null)
            inputFondo.color = new Color(0.08f, 0.095f, 0.095f, 0.96f);

        TMP_Text textoInput = inputRespuesta.textComponent;

        if (textoInput != null)
        {
            textoInput.color = new Color(0.95f, 1f, 0.95f, 1f);
            textoInput.fontSize = 32f;
            textoInput.alignment = TextAlignmentOptions.MidlineLeft;
            textoInput.margin = new Vector4(22f, 8f, 22f, 8f);
        }

        if (inputRespuesta.placeholder is TMP_Text placeholder)
        {
            placeholder.text = "Escribe tu respuesta...";
            placeholder.color = new Color(0.55f, 0.68f, 0.62f, 0.75f);
            placeholder.fontSize = 30f;
            placeholder.alignment = TextAlignmentOptions.MidlineLeft;
            placeholder.margin = new Vector4(22f, 8f, 22f, 8f);
        }

        inputRespuesta.caretColor = new Color(0.2f, 1f, 0.55f, 1f);
        inputRespuesta.selectionColor = new Color(0.2f, 0.7f, 0.45f, 0.45f);
    }

    private void EstilizarBoton()
    {
        Button boton = GetComponentInChildren<Button>(true);

        if (boton == null)
            return;

        RectTransform botonRect = boton.GetComponent<RectTransform>();
        botonRect.anchorMin = new Vector2(0.5f, 0f);
        botonRect.anchorMax = new Vector2(0.5f, 0f);
        botonRect.pivot = new Vector2(0.5f, 0.5f);

        // 🔥 BAJADO PARA QUE NO SE PISE CON EL INPUT
        botonRect.anchoredPosition = new Vector2(0f, 60f);
        botonRect.sizeDelta = new Vector2(320f, 80f);

        Image botonFondo = boton.GetComponent<Image>();

        if (botonFondo != null)
            botonFondo.color = new Color(0.12f, 0.42f, 0.28f, 1f);

        ColorBlock colores = boton.colors;
        colores.normalColor = new Color(0.12f, 0.42f, 0.28f, 1f);
        colores.highlightedColor = new Color(0.18f, 0.58f, 0.38f, 1f);
        colores.pressedColor = new Color(0.08f, 0.28f, 0.2f, 1f);
        colores.selectedColor = colores.highlightedColor;
        colores.disabledColor = new Color(0.12f, 0.16f, 0.14f, 0.55f);
        boton.colors = colores;

        TMP_Text textoBoton = boton.GetComponentInChildren<TMP_Text>(true);

        if (textoBoton != null)
        {
            textoBoton.text = textoBotonEnviar;
            textoBoton.color = new Color(0.96f, 1f, 0.96f, 1f);
            textoBoton.fontSize = 24f;
            textoBoton.fontStyle = FontStyles.Bold;
            textoBoton.alignment = TextAlignmentOptions.Center;
        }
    }

    private IEnumerator Fundido(float objetivo)
    {
        if (panelTransicion == null || duracionFundido <= 0f)
        {
            if (panelTransicion != null)
                panelTransicion.alpha = objetivo;

            yield break;
        }

        panelTransicion.blocksRaycasts = true;

        float inicio = panelTransicion.alpha;
        float tiempo = 0f;

        while (tiempo < duracionFundido)
        {
            tiempo += Time.unscaledDeltaTime;
            panelTransicion.alpha = Mathf.Lerp(inicio, objetivo, tiempo / duracionFundido);
            yield return null;
        }

        panelTransicion.alpha = objetivo;
        panelTransicion.blocksRaycasts = objetivo > 0f;
    }

    private void PrepararPanelTransicion()
    {
        if (panelTransicion != null)
            return;

        Canvas canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
            return;

        GameObject panel = new GameObject("PanelTransicionCamara");
        panel.transform.SetParent(canvas.transform, false);
        panel.transform.SetAsLastSibling();

        Image imagen = panel.AddComponent<Image>();
        imagen.color = Color.black;

        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        panelTransicion = panel.AddComponent<CanvasGroup>();
        panelTransicion.alpha = 0f;
        panelTransicion.interactable = false;
        panelTransicion.blocksRaycasts = false;
    }

    public void Cerrar()
    {
        alResponderCorrecto = null;
        EstaAbierto = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
        MostrarDialogo(false);
        gameObject.SetActive(false);
    }
}
