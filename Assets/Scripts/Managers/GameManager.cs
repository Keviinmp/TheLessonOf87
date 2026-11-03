using TMPro;
using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TMP_Text vidasText;

    [Header("Sistema de pruebas")]
    public int indicePruebaActual = 0;

    [Header("Jumpscare por vídeo")]
    public VideoPlayer videoJumpscare;
    public VideoClip[] videosPorPrueba;
    public GameObject pantallaJumpscare;

    [Header("Pantalla de muerte (CANVAS)")]
    public GameObject canvasMuerte;
    public TMP_Text textoErrores;

    [Header("Vidas")]
    public int vidas = 3;

    // Estado
    private bool muerto = false;

    // ERRORES
    private List<string> errores = new List<string>();

    // TIEMPO PARTIDA
    private float tiempoInicio;

    #region INICIO PARTIDA

    public void IniciarPartida()
    {
        tiempoInicio = Time.time;
        muerto = false;
        errores.Clear();
        vidas = 3;
        ActualizarUI();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ActualizarUI();

        if (pantallaJumpscare != null)
            pantallaJumpscare.SetActive(false);

        if (canvasMuerte != null)
            canvasMuerte.SetActive(false);
    }

    #endregion

    #region TIEMPO

    public string ObtenerTiempoFormateado()
    {
        float tiempo = Time.time - tiempoInicio;

        int minutos = Mathf.FloorToInt(tiempo / 60f);
        int segundos = Mathf.FloorToInt(tiempo % 60f);

        return $"{minutos:00}:{segundos:00}";
    }

    #endregion

    #region ERRORES

    public void RegistrarError(string error)
    {
        errores.Add(error);
    }

    void MostrarErrores()
    {
        if (textoErrores == null)
            return;

        textoErrores.text = "";
        textoErrores.lineSpacing = 275f;

        foreach (string e in errores)
        {
            textoErrores.text += "- " + e + "\n";
        }
    }

    #endregion

    #region VIDA

    public void PerderVida()
    {
        if (muerto)
            return;

        vidas--;
        ActualizarUI();

        if (vidas <= 0)
            Morir();
    }

    public void SumarVida(int cantidad = 1)
    {
    if (muerto)
        return;

    vidas += cantidad;

    // opcional → limitar vidas máximas
    vidas = Mathf.Clamp(vidas, 0, 10);

    ActualizarUI();
    }

    void ActualizarUI()
    {
        if (vidasText == null)
            return;

        string corazones = "";

        for (int i = 0; i < vidas; i++)
            corazones += "♥";

        vidasText.text = corazones;
    }

    #endregion

    #region MUERTE

    private void Morir()
    {
        if (muerto)
            return;

        muerto = true;

        Time.timeScale = 0f;

        int i = indicePruebaActual;

        if (pantallaJumpscare != null)
            pantallaJumpscare.SetActive(true);

        if (
            videoJumpscare == null ||
            videosPorPrueba == null ||
            i < 0 ||
            i >= videosPorPrueba.Length
        )
        {
            MostrarPantallaMuerte();
            return;
        }

        videoJumpscare.loopPointReached -= VideoAcabado;
        videoJumpscare.loopPointReached += VideoAcabado;

        StartCoroutine(PlayJumpscare(i));
    }

    private IEnumerator PlayJumpscare(int i)
    {
        videoJumpscare.Stop();
        videoJumpscare.clip = videosPorPrueba[i];

        yield return null;

        videoJumpscare.Play();
    }

    private void VideoAcabado(VideoPlayer vp)
    {
        if (videoJumpscare != null)
            videoJumpscare.Stop();

        if (pantallaJumpscare != null)
            pantallaJumpscare.SetActive(false);

        MostrarPantallaMuerte();
    }

    void MostrarPantallaMuerte()
    {
        if (canvasMuerte == null)
            return;

        canvasMuerte.SetActive(true);
        canvasMuerte.transform.SetAsLastSibling();

        MostrarErrores();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    #endregion

    #region MENU

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }

    #endregion
}