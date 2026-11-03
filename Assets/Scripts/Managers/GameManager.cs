using TMPro;
using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona el estado global de la partida: vidas, errores, tiempo, muerte con jumpscare y vuelta al menú. Usa el patrón Singleton.
/// </summary>
public class GameManager : MonoBehaviour {
    /// <summary>Instancia global (Singleton) del GameManager.</summary>
    public static GameManager Instance;

    /// <summary>Texto que muestra las vidas restantes.</summary>
    [Header("UI")]

    public TMP_Text vidasText;

    /// <summary>Índice de la prueba activa; determina qué jumpscare se reproduce al morir.</summary>
    [Header("Sistema de pruebas")]

    public int indicePruebaActual = 0;

    /// <summary>Reproductor del vídeo de jumpscare.</summary>
    [Header("Jumpscare por vídeo")]

    public VideoPlayer videoJumpscare;

    /// <summary>Vídeo de jumpscare asociado a cada prueba.</summary>
    public VideoClip[] videosPorPrueba;

    /// <summary>Superficie donde se reproduce el vídeo.</summary>
    public GameObject pantallaJumpscare;

    /// <summary>Canvas que se muestra tras perder la partida.</summary>
    [Header("Pantalla de muerte (CANVAS)")]

    public GameObject canvasMuerte;

    /// <summary>Texto donde se listan los errores cometidos.</summary>
    public TMP_Text textoErrores;

    /// <summary>Número actual de vidas.</summary>
    [Header("Vidas")]

    public int vidas = 3;


    /// <summary>Indica si el jugador ya ha muerto.</summary>
    private bool muerto = false;

    /// <summary>Historial de errores cometidos en la partida.</summary>
    private List<string> errores = new List<string>();

    /// <summary>Instante (<c>Time.time</c>) en el que comenzó la partida.</summary>
    private float tiempoInicio;


    #region INICIO PARTIDA

    /// <summary>Reinicia el estado de la partida: tiempo, vidas, errores y estado de muerte.</summary>
    public void IniciarPartida() {
        tiempoInicio = Time.time;

        muerto = false;

        errores.Clear();

        vidas = 3;

        ActualizarUI();
    }


    /// <summary>Inicializa el patrón Singleton; destruye el objeto si ya existe otra instancia.</summary>
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }


    /// <summary>Actualiza la UI de vidas y oculta las pantallas de jumpscare y muerte.</summary>
    private void Start() {
        ActualizarUI();

        if (pantallaJumpscare != null) {
            pantallaJumpscare.SetActive(false);
        }

        if (canvasMuerte != null) {
            canvasMuerte.SetActive(false);
        }
    }

    #endregion


    #region TIEMPO

    /// <summary>
    /// Calcula el tiempo jugado desde el inicio de la partida.
    /// </summary>
    /// <returns>Tiempo transcurrido en formato mm:ss.</returns>
    public string ObtenerTiempoFormateado() {
        float tiempo = Time.time - tiempoInicio;

        int minutos =
            Mathf.FloorToInt(tiempo / 60f);

        int segundos =
            Mathf.FloorToInt(tiempo % 60f);

        return $"{minutos:00}:{segundos:00}";
    }

    #endregion


    #region ERRORES

    /// <summary>
    /// Guarda un error cometido durante la partida.
    /// </summary>
    /// <param name="error">Descripción del error (pregunta, respuesta correcta y respuesta del jugador).</param>
    public void RegistrarError(string error) {
        errores.Add(error);
    }


    /// <summary>Muestra en pantalla la lista de errores registrados.</summary>
    private void MostrarErrores() {

        if (textoErrores == null) {
            return;
        }

        textoErrores.text = "";

        textoErrores.lineSpacing = 200f;

        foreach (string e in errores) {
            textoErrores.text += "- " + e + "\n";
        }
    }

    #endregion


    #region VIDA

    /// <summary>Resta una vida y lanza la muerte si el jugador se queda sin ninguna.</summary>
    public void PerderVida() {

        if (muerto) {
            return;
        }

        vidas--;

        ActualizarUI();

        if (vidas <= 0) {
            Morir();
        }
    }


    /// <summary>
    /// Suma vidas al jugador, con un máximo de 10.
    /// </summary>
    /// <param name="cantidad">Número de vidas a sumar.</param>
    public void SumarVida(int cantidad = 1) {

        if (muerto) {
            return;
        }

        vidas += cantidad;

        // Limita cantidad máxima
        vidas = Mathf.Clamp(vidas, 0, 10);

        ActualizarUI();
    }


    /// <summary>Actualiza el texto de vidas con un corazón por cada vida.</summary>
    private void ActualizarUI() {

        if (vidasText == null) {
            return;
        }

        string corazones = "";

        for (int i = 0; i < vidas; i++) {
            corazones += "♥";
        }

        vidasText.text = corazones;
    }

    #endregion


    #region MUERTE

    /// <summary>
    /// Ejecuta el flujo de muerte: congela el juego y reproduce el jumpscare de la prueba actual (o muestra directamente la pantalla de muerte si no hay vídeo).
    /// </summary>
    private void Morir() {

        if (muerto) {
            return;
        }

        muerto = true;

        // Congela el juego
        Time.timeScale = 0f;

        int i = indicePruebaActual;

        if (pantallaJumpscare != null) {
            pantallaJumpscare.SetActive(true);
        }

        // Si no existe vídeo válido muestra muerte directa
        if (
            videoJumpscare == null ||
            videosPorPrueba == null ||
            i < 0 ||
            i >= videosPorPrueba.Length
        ) {
            MostrarPantallaMuerte();
            return;
        }

        // Reinicia suscripción del evento de vídeo
        videoJumpscare.loopPointReached -= VideoAcabado;
        videoJumpscare.loopPointReached += VideoAcabado;

        StartCoroutine(
            PlayJumpscare(i)
        );
    }


    /// <summary>
    /// Prepara y reproduce el vídeo de jumpscare.
    /// </summary>
    /// <param name="i">Índice de la prueba cuyo vídeo se reproduce.</param>
    private IEnumerator PlayJumpscare(int i) {

        videoJumpscare.Stop();

        videoJumpscare.clip =
            videosPorPrueba[i];

        yield return null;

        videoJumpscare.Play();
    }


    /// <summary>
    /// Se ejecuta al terminar el vídeo: lo detiene, oculta la pantalla y muestra la pantalla de muerte.
    /// </summary>
    /// <param name="vp">Reproductor que ha terminado.</param>
    private void VideoAcabado(VideoPlayer vp) {

        if (videoJumpscare != null) {
            videoJumpscare.Stop();
        }

        if (pantallaJumpscare != null) {
            pantallaJumpscare.SetActive(false);
        }

        MostrarPantallaMuerte();
    }


    /// <summary>Muestra la interfaz de fin de partida y libera el cursor.</summary>
    private void MostrarPantallaMuerte() {

        if (canvasMuerte == null) {
            return;
        }

        canvasMuerte.SetActive(true);

        canvasMuerte.transform.SetAsLastSibling();

        MostrarErrores();

        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;
    }

    #endregion

    #region MENU

    /// <summary>Restaura el tiempo del juego y carga la escena del menú principal.</summary>
    public void VolverAlMenu() {

        Time.timeScale = 1f;

        SceneManager.LoadScene("MenuInicio");
    }

    #endregion
}