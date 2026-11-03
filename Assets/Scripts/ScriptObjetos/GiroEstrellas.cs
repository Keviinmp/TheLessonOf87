using UnityEngine;

/// <summary>Hace girar el objeto de forma continua sobre el eje Z.</summary>
public class GiroEstrellas : MonoBehaviour
{
    /// <summary>Velocidad de rotación, en grados por segundo.</summary>
    public float velocidad = 500f;

    /// <summary>Rota el objeto cada frame, de forma independiente del framerate.</summary>
    void Update() {
        // Rotación continua sobre el eje Z
        // Time.deltaTime garantiza independencia del framerate
        transform.Rotate(0, 0, Time.deltaTime * velocidad);
    }
}