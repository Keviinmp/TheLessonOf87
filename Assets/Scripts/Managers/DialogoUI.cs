using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using System;

/// <summary>
/// Interfaz de diálogo que muestra una pregunta, recoge la respuesta del jugador y gestiona la transición de cámara tras acertar.
/// </summary>
public class DialogoUI : MonoBehaviour
{
    /// <summary>Indica si el diálogo está activo (evita abrir varios a la vez).</summary>
    public static bool EstaAbierto { get; private set; }

    /// <summary>Texto donde se muestra la pregunta.</summary>
    public TMP_Text textoPregunta;
    /// <summary>Campo donde el jugador escribe su respuesta.</summary>
    public TMP_InputField inputRespuesta;

    /// <summary>Cámara del evento que se muestra tras responder bien.</summary>
    public Camera camaraPuerta;
    /// <summary>Cámara del jugador.</summary>
    public Camera camaraJugador;

    /// <summary>Panel negro usado para el fundido entre cámaras.</summary>
    [Header("Transicion de camara")]

    public CanvasGroup panelTransicion;

    /// <summary>Duración del fundido, en segundos.</summary>
    public float duracionFundido = 0.5f;

    /// <summary>Segundos que permanece activa la cámara del evento.</summary>
    public float tiempoCamaraPuerta = 3f;

    /// <summary>Si es <c>true</c>, aplica por código el estilo visual del diálogo.</summary>
    [Header("Estilo visual")]

    public bool aplicarEstiloAutomatico = true;

    /// <summary>Texto del botón de envío.</summary>
    public string textoBotonEnviar = "Responder";


    /// <summary>Respuesta correcta de la pregunta actual.</summary>
    private string respuestaCorrecta;

    /// <summary>Acción que se ejecuta cuando el jugador responde correctamente.</summary>
    private Action alResponderCorrecto;

    /// <summary>CanvasGroup principal del diálogo.</summary>
    private CanvasGroup canvasGroup;


    /// <summary>
    /// Obtiene (o crea) el CanvasGroup, aplica el estilo visual y prepara el panel de transición.
    /// </summary>
    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null) {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (aplicarEstiloAutomatico) {
            AplicarEstiloVisual();
        }

        PrepararPanelTransicion();
    }


    /// <summary>
    /// Abre el diálogo con una pregunta, libera el cursor y pausa el juego.
    /// </summary>
    /// <param name="pregunta">Enunciado a mostrar.</param>
    /// <param name="respuesta">Respuesta correcta esperada.</param>
    /// <param name="alResponderCorrecto">Acción opcional que se ejecuta si el jugador acierta.</param>
    public void Abrir(string pregunta, string respuesta, Action alResponderCorrecto = null) {

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


    /// <summary>
    /// Comprueba la respuesta del jugador sin distinguir mayúsculas. Si acierta, lanza la transición de cámara; si falla, registra el error y resta una vida.
    /// </summary>
    public void Enviar() {

        string respuestaJugador = inputRespuesta.text.Trim();

        // Caso correcto
        if (respuestaJugador.ToLower() == respuestaCorrecta.ToLower()) {

            Debug.Log("Correcto!");

            MostrarDialogo(false);

            StartCoroutine(MostrarCamaraPuertaYCerrar());

            return;
        }

        // Caso incorrecto
        Debug.Log(
            $"Incorrecto. La respuesta correcta era: {respuestaCorrecta}"
        );

        // Método que registra el error en el GameManager global
        if (GameManager.Instance != null) {
            GameManager.Instance.RegistrarError(
                "Pregunta: " + textoPregunta.text +
                " | Correcta: " + respuestaCorrecta +
                " | Tú: " + respuestaJugador
            );
        }

        // Método que reduce una vida del jugador
        GameManager.Instance.PerderVida();

        Cerrar();
    }


    /// <summary>
    /// Muestra u oculta el panel del diálogo.
    /// </summary>
    /// <param name="visible"><c>true</c> para mostrarlo.</param>
    private void MostrarDialogo(bool visible) {

        canvasGroup.alpha = visible ? 1f : 0f;

        canvasGroup.interactable = visible;

        canvasGroup.blocksRaycasts = visible;
    }


    /// <summary>
    /// Tras una respuesta correcta, hace un fundido a la cámara del evento, ejecuta la acción de acierto, vuelve a la cámara del jugador y cierra el diálogo.
    /// </summary>
    private IEnumerator MostrarCamaraPuertaYCerrar() {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        yield return Fundido(1f);

        if (camaraJugador != null) {
            camaraJugador.enabled = false;
        }

        if (camaraPuerta != null) {
            camaraPuerta.enabled = true;
        }

        alResponderCorrecto?.Invoke();

        yield return Fundido(0f);

        yield return new WaitForSecondsRealtime(tiempoCamaraPuerta);

        yield return Fundido(1f);

        if (camaraPuerta != null) {
            camaraPuerta.enabled = false;
        }

        if (camaraJugador != null) {
            camaraJugador.enabled = true;
        }

        yield return Fundido(0f);

        Cerrar();
    }


    /// <summary>Aplica por código el tamaño, los colores y la tipografía del panel y de la pregunta.</summary>
    private void AplicarEstiloVisual() {

        RectTransform panelRect = GetComponent<RectTransform>();

        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(820f, 430f);

        Image fondo = GetComponent<Image>();

        if (fondo == null) {
            fondo = gameObject.AddComponent<Image>();
        }

        fondo.color = new Color(0.035f, 0.04f, 0.045f, 0.96f);
        fondo.raycastTarget = true;

        // Configuración del texto de la pregunta
        if (textoPregunta != null) {

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


    /// <summary>Ajusta posición, colores y textos del campo de respuesta.</summary>
    private void EstilizarInput() {

        if (inputRespuesta == null) {
            return;
        }

        RectTransform inputRect = inputRespuesta.GetComponent<RectTransform>();

        inputRect.anchorMin = new Vector2(0.5f, 0.5f);
        inputRect.anchorMax = new Vector2(0.5f, 0.5f);
        inputRect.pivot = new Vector2(0.5f, 0.5f);

        inputRect.anchoredPosition = new Vector2(0f, -10f);
        inputRect.sizeDelta = new Vector2(850f, 80f);

        Image inputFondo = inputRespuesta.GetComponent<Image>();

        if (inputFondo != null) {
            inputFondo.color = new Color(0.08f, 0.095f, 0.095f, 0.96f);
        }

        TMP_Text textoInput = inputRespuesta.textComponent;

        if (textoInput != null) {
            textoInput.color = new Color(0.95f, 1f, 0.95f, 1f);
            textoInput.fontSize = 32f;
            textoInput.alignment = TextAlignmentOptions.MidlineLeft;
            textoInput.margin = new Vector4(22f, 8f, 22f, 8f);
        }

        if (inputRespuesta.placeholder is TMP_Text placeholder) {
            placeholder.text = "Escribe tu respuesta...";
            placeholder.color = new Color(0.55f, 0.68f, 0.62f, 0.75f);
            placeholder.fontSize = 30f;
            placeholder.alignment = TextAlignmentOptions.MidlineLeft;
            placeholder.margin = new Vector4(22f, 8f, 22f, 8f);
        }

        inputRespuesta.caretColor = new Color(0.2f, 1f, 0.55f, 1f);
        inputRespuesta.selectionColor = new Color(0.2f, 0.7f, 0.45f, 0.45f);
    }


    /// <summary>Ajusta posición, colores y texto del botón de envío.</summary>
    private void EstilizarBoton() {

        Button boton = GetComponentInChildren<Button>(true);

        if (boton == null) {
            return;
        }

        RectTransform botonRect = boton.GetComponent<RectTransform>();

        botonRect.anchorMin = new Vector2(0.5f, 0f);
        botonRect.anchorMax = new Vector2(0.5f, 0f);
        botonRect.pivot = new Vector2(0.5f, 0.5f);

        botonRect.anchoredPosition = new Vector2(0f, 60f);
        botonRect.sizeDelta = new Vector2(320f, 80f);

        Image botonFondo = boton.GetComponent<Image>();

        if (botonFondo != null) {
            botonFondo.color = new Color(0.12f, 0.42f, 0.28f, 1f);
        }

        ColorBlock colores = boton.colors;

        colores.normalColor = new Color(0.12f, 0.42f, 0.28f, 1f);
        colores.highlightedColor = new Color(0.18f, 0.58f, 0.38f, 1f);
        colores.pressedColor = new Color(0.08f, 0.28f, 0.2f, 1f);
        colores.selectedColor = colores.highlightedColor;
        colores.disabledColor = new Color(0.12f, 0.16f, 0.14f, 0.55f);

        boton.colors = colores;

        TMP_Text textoBoton = boton.GetComponentInChildren<TMP_Text>(true);

        if (textoBoton != null) {
            textoBoton.text = textoBotonEnviar;
            textoBoton.color = new Color(0.96f, 1f, 0.96f, 1f);
            textoBoton.fontSize = 24f;
            textoBoton.fontStyle = FontStyles.Bold;
            textoBoton.alignment = TextAlignmentOptions.Center;
        }
    }


    /// <summary>
    /// Anima la opacidad del panel de transición hasta el valor indicado.
    /// </summary>
    /// <param name="objetivo">Opacidad final (de 0 a 1).</param>
    private IEnumerator Fundido(float objetivo) {

        if (panelTransicion == null || duracionFundido <= 0f) {

            if (panelTransicion != null) {
                panelTransicion.alpha = objetivo;
            }

            yield break;
        }

        panelTransicion.blocksRaycasts = true;

        float inicio = panelTransicion.alpha;
        float tiempo = 0f;

        while (tiempo < duracionFundido) {
            tiempo += Time.unscaledDeltaTime;

            panelTransicion.alpha =
                Mathf.Lerp(inicio, objetivo, tiempo / duracionFundido);

            yield return null;
        }

        panelTransicion.alpha = objetivo;
        panelTransicion.blocksRaycasts = objetivo > 0f;
    }


    /// <summary>Crea el panel negro de transición si no existe en la escena.</summary>
    private void PrepararPanelTransicion() {

        if (panelTransicion != null) {
            return;
        }

        Canvas canvas = GetComponentInParent<Canvas>();

        if (canvas == null) {
            return;
        }

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


    /// <summary>Cierra el diálogo, bloquea el cursor y restaura el tiempo del juego.</summary>
    public void Cerrar() {

        alResponderCorrecto = null;

        EstaAbierto = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        MostrarDialogo(false);

        gameObject.SetActive(false);
    }
}