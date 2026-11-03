using UnityEngine;
using UnityEngine.UI;

public class CrosshairLEDController : MonoBehaviour
{
    [Header("Configuración UI")]
    public Camera playerCamera;     // Tu cámara principal
    public float maxDistance = 200f; // Alcance del raycast

    public Image crosshairLED;      // El borde LED (UI Image)
    
    void Start()
    {
        // Apagar el LED al inicio
        if (crosshairLED != null)
            crosshairLED.enabled = false;
    }

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

    bool EsInteractuable(Collider collider)
    {
        return collider.GetComponentInParent<Interactuable>() != null ||
               collider.tag == "interactuable" ||
               collider.tag == "Interactable";
    }
}