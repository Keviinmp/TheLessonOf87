using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float distanciaMirar = 6f;
    public float distanciaInteractuar = 6f;

    private Outline outlineActual;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        if (cam == null)
            Debug.LogError("No hay cámara con tag MainCamera");
    }

    void Update()
    {
        if (cam == null) return;

        if (DialogoUI.EstaAbierto)
        {
            DisableOutline();
            return;
        }

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        float distanciaMax = Mathf.Max(distanciaMirar, distanciaInteractuar);

        if (Physics.Raycast(ray, out hit, distanciaMax))
        {
            Interactuable interactuable = hit.collider.GetComponentInParent<Interactuable>();

            bool tieneScript = interactuable != null;
            bool tieneTag = hit.collider.CompareTag("Interactuable");

            // 🔥 CASO 1: TIENE SCRIPT (interacción completa)
            if (tieneScript)
            {
                HandleOutline(hit);

                float distanciaAlObjeto =
                    Vector3.Distance(transform.position, interactuable.transform.position);

                if (distanciaAlObjeto <= distanciaInteractuar &&
                    Input.GetKeyDown(KeyCode.E))
                {
                    interactuable.Interactuar();
                }

                return;
            }

            // ✨ CASO 2: SOLO TAG (solo outline, sin interacción)
            if (tieneTag)
            {
                HandleOutline(hit);
                return;
            }
        }

        DisableOutline();
    }

    void HandleOutline(RaycastHit hit)
    {
        Outline outline = hit.collider.GetComponentInParent<Outline>();

        if (outline == null)
            outline = hit.collider.GetComponentInChildren<Outline>();

        if (outlineActual != outline)
        {
            if (outlineActual != null)
                outlineActual.enabled = false;

            outlineActual = outline;

            if (outlineActual != null)
                outlineActual.enabled = true;
        }
    }

    void DisableOutline()
    {
        if (outlineActual != null)
        {
            outlineActual.enabled = false;
            outlineActual = null;
        }
    }
}