using UnityEngine;

/// <summary>Hace girar el ventilador de forma continua sobre el eje Z.</summary>
public class GiroVentilador : MonoBehaviour
{
    /// <summary>Velocidad de rotación, en grados por segundo.</summary>
    public float velocidad = 500f;

    /// <summary>Rota el ventilador cada frame, de forma independiente del framerate.</summary>
    void Update() {
        // Rotación continua en el eje Z, Time.deltaTime hace que la rotación sea independiente del framerate
        transform.Rotate(0, 0, velocidad * Time.deltaTime);
    }
}