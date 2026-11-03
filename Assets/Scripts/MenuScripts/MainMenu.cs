using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controla el menú principal: inicio de partida, opciones de volumen y resolución, y salida del juego.
/// </summary>
public class MainMenu : MonoBehaviour {
    /// <summary>Panel del menú principal.</summary>
    public GameObject menuPrincipal;

    /// <summary>Panel de opciones del juego.</summary>
    public GameObject panelOpciones;

    /// <summary>Slider que controla el volumen general.</summary>
    public Slider sliderVolumen;

    /// <summary>Desplegable para elegir la resolución.</summary>
    public TMP_Dropdown dropdownResolucion;


    /// <summary>
    /// Muestra el menú principal y carga el volumen y la resolución guardados en <c>PlayerPrefs</c>.
    /// </summary>
    void Start() {

        menuPrincipal.SetActive(true);
        panelOpciones.SetActive(false);

        // VOLUMEN

        // Carga el volumen guardado previamente en PlayerPrefs
        float volumenGuardado = PlayerPrefs.GetFloat("volume", 1f);

        // Aplica el volumen global del juego
        AudioListener.volume = volumenGuardado;

        // Sincroniza el slider con el valor guardado
        sliderVolumen.value = volumenGuardado;

        // Listener para actualizar volumen en tiempo real
        sliderVolumen.onValueChanged.AddListener(CambiarVolumen);


        // RESOLUCIÓN

        if (dropdownResolucion != null) {

            // Listener que ejecuta el cambio de resolución al seleccionar opción
            dropdownResolucion.onValueChanged.AddListener(CambiarResolucion);

            // Carga resolución guardada (por defecto 2)
            int res = PlayerPrefs.GetInt("resolution", 2);

            dropdownResolucion.value = res;

            dropdownResolucion.RefreshShownValue();

            // Aplica la resolución al iniciar
            CambiarResolucion(res);
        }
    }


    /// <summary>Inicia una nueva partida y carga la escena de juego.</summary>
    public void IniciarJuego() {

        if (GameManager.Instance != null) {
            GameManager.Instance.IniciarPartida();
        }

        SceneManager.LoadScene("MainScene");
    }


    /// <summary>Abre el panel de opciones.</summary>
    public void AbrirOpciones() {
        menuPrincipal.SetActive(false);
        panelOpciones.SetActive(true);
    }


    /// <summary>Cierra el panel de opciones y vuelve al menú principal.</summary>
    public void CerrarOpciones() {
        panelOpciones.SetActive(false);
        menuPrincipal.SetActive(true);
    }


    /// <summary>
    /// Cambia el volumen global del juego y lo guarda.
    /// </summary>
    /// <param name="volumen">Volumen entre 0 y 1.</param>
    public void CambiarVolumen(float volumen) {

        AudioListener.volume = volumen;

        PlayerPrefs.SetFloat("volume", volumen);
        PlayerPrefs.Save();
    }


    /// <summary>
    /// Cambia la resolución según la opción elegida y la guarda.
    /// </summary>
    /// <param name="index">Opción elegida: 0 = 1280x720 en ventana, 1 = 1920x1080 en ventana, 2 = 1920x1080 a pantalla completa.</param>
    public void CambiarResolucion(int index) {

        switch (index) {

            case 0:
                Screen.SetResolution(1280, 720, false);
                break;

            case 1:
                Screen.SetResolution(1920, 1080, false);
                break;

            case 2:
                Screen.SetResolution(1920, 1080, true);
                break;
        }

        PlayerPrefs.SetInt("resolution", index);
        PlayerPrefs.Save();

        Debug.Log("Resolución cambiada: " + index);
    }


    /// <summary>Cierra la aplicación.</summary>
    public void CerrarJuego() {

        Debug.Log("BOTÓN CERRAR FUNCIONANDO");

        Application.Quit();
    }
}