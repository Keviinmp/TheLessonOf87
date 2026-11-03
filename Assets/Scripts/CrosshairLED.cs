using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla un indicador LED en pantalla que se enciende cuando el jugador mira un objeto interactuable.
/// </summary>
public class CrosshairLEDController : MonoBehaviour
{
    /// <summary>Cámara principal desde la que se lanza el raycast.</summary>
    [Header("Configuración UI")]
    public Camera playerCamera;
    /// <summary>Alcance máximo del raycast, en unidades de Unity.</summary>
    public float maxDistance = 200f;

    /// <summary>Imagen de UI que hace de borde LED de la mira.</summary>
    public Image crosshairLED;
    
    /// <summary>Apaga el LED al iniciar la escena.</summary>
    void Start()
    {
        // Apagar el LED al inicio
        if (crosshairLED != null)
            crosshairLED.enabled = false;
    }

    /// <summary>
    /// Lanza un raycast desde la cámara cada frame y enciende o apaga el LED según se apunte o no a un objeto interactuable.
    /// </summary>
    void Update()
    {
        if (crosshairLED == null || playerCamera == null)
            return;

        // Raycast desde la cámara hacia adelante
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        bool mirandoInteractuable = Physics.Raycast(ray, out RaycastHit hit, maxDistance) &&
                                    EsInteractuable(hit.collider);

        if (mirandoInteractuable)
        {
            // Enciende el LED
            crosshairLED.enabled = true;
        }
        else
        {
            // Apaga el LED
            crosshairLED.enabled = false;
        }
    }

    /// <summary>
    /// Indica si el collider pertenece a un objeto interactuable (por componente o por tag).
    /// </summary>
    /// <param name="collider">Collider impactado por el raycast.</param>
    /// <returns><c>true</c> si el objeto es interactuable.</returns>
    bool EsInteractuable(Collider collider)
    {
        return collider.GetComponentInParent<Interactuable>() != null ||
               collider.tag == "interactuable" ||
               collider.tag == "Interactable";
    }
}