using UnityEngine;

/// <summary>
/// Detecta con un raycast central el objeto apuntado, resalta su contorno y permite interactuar con la tecla E.
/// </summary>
public class PlayerInteractor : MonoBehaviour {

    /// <summary>Distancia máxima para detectar el objeto apuntado (feedback visual).</summary>
    public float distanciaMirar = 6f;

    /// <summary>Distancia máxima para poder interactuar con la tecla E.</summary>
    public float distanciaInteractuar = 6f;

    /// <summary>Contorno actualmente activo en el objeto apuntado.</summary>
    private Outline outlineActual;

    /// <summary>Cámara principal del jugador, usada para el raycast.</summary>
    private Camera cam;


    /// <summary>Obtiene la cámara principal.</summary>
    void Start() {

        cam = Camera.main;

        if (cam == null)
            Debug.LogError("No hay cámara con tag MainCamera");
    }


    /// <summary>
    /// Lanza un raycast desde el centro de la pantalla: resalta el objeto apuntado y, si es interactuable y está en rango, permite interactuar con E.
    /// </summary>
    void Update() {

        if (cam == null) return;

        // Bloquea interacción si el diálogo está activo
        if (DialogoUI.EstaAbierto) {
            DisableOutline();
            return;
        }

        // Ray desde el centro de la pantalla (crosshair invisible)
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        // Distancia máxima de detección
        float distanciaMax = Mathf.Max(distanciaMirar, distanciaInteractuar);

        // Si el raycast impacta un objeto dentro del rango
        if (Physics.Raycast(ray, out hit, distanciaMax)) {

            // Intenta obtener componente interactuable del objeto impactado
            Interactuable interactuable = hit.collider.GetComponentInParent<Interactuable>();

            bool tieneScript = interactuable != null;
            bool tieneTag = hit.collider.CompareTag("Interactuable");

            // CASO 1: objeto con sistema interactuable completo
            if (tieneScript) {

                HandleOutline(hit);

                float distanciaAlObjeto =
                    Vector3.Distance(transform.position, interactuable.transform.position);

                // Permite interacción solo si está dentro del rango y se pulsa E
                if (distanciaAlObjeto <= distanciaInteractuar &&
                    Input.GetKeyDown(KeyCode.E)) {

                    interactuable.Interactuar();
                }

                return;
            }

            // CASO 2: objeto solo con tag (solo feedback visual sin interacción)
            if (tieneTag) {
                HandleOutline(hit);
                return;
            }
        }

        // Si no hay objetos en mira, se desactiva el outline
        DisableOutline();
    }


    /// <summary>
    /// Activa el contorno del objeto apuntado y desactiva el anterior.
    /// </summary>
    /// <param name="hit">Resultado del raycast.</param>
    void HandleOutline(RaycastHit hit) {

        Outline outline = hit.collider.GetComponentInParent<Outline>();

        if (outline == null)
            outline = hit.collider.GetComponentInChildren<Outline>();

        // Evita reactivar el mismo outline continuamente
        if (outlineActual != outline) {

            if (outlineActual != null)
                outlineActual.enabled = false;

            outlineActual = outline;

            if (outlineActual != null)
                outlineActual.enabled = true;
        }
    }


    /// <summary>Desactiva el contorno activo.</summary>
    void DisableOutline() {

        if (outlineActual != null) {

            outlineActual.enabled = false;
            outlineActual = null;
        }
    }
}