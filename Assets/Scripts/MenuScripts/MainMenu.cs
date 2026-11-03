using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject panelOpciones;

    public Slider sliderVolumen;

    // Resolución 
    public TMP_Dropdown dropdownResolucion;

    void Start()
    {
        menuPrincipal.SetActive(true);
        panelOpciones.SetActive(false);

        // 🔊 Volumen
        float volumenGuardado = PlayerPrefs.GetFloat("volume", 1f);
        AudioListener.volume = volumenGuardado;
        sliderVolumen.value = volumenGuardado;
        sliderVolumen.onValueChanged.AddListener(CambiarVolumen);

        // 📺 Resolución
        if (dropdownResolucion != null)
        {
            dropdownResolucion.onValueChanged.AddListener(CambiarResolucion);

            int res = PlayerPrefs.GetInt("resolution", 2);
            dropdownResolucion.value = res;

            dropdownResolucion.RefreshShownValue();

            CambiarResolucion(res);
        }
    }

    public void IniciarJuego()
    {
    if (GameManager.Instance != null)
    {
        GameManager.Instance.IniciarPartida();
    }

    SceneManager.LoadScene("MainScene");
    }

    public void AbrirOpciones()
    {
        menuPrincipal.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    // VOLUMEN
    public void CambiarVolumen(float volumen)
    {
        AudioListener.volume = volumen;
        PlayerPrefs.SetFloat("volume", volumen);
        PlayerPrefs.Save();
    }

    // RESOLUCIÓN
    public void CambiarResolucion(int index)
    {
        switch (index)
        {
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

    public void CerrarJuego()
    {
        Debug.Log("BOTÓN CERRAR FUNCIONANDO");
        Application.Quit();
    }
}