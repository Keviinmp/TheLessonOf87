using UnityEngine;

/// <summary>Hace girar el objeto de forma continua sobre el eje X.</summary>
public class GiroMusicBox : MonoBehaviour
{
    /// <summary>Velocidad de rotación, en grados por segundo.</summary>
    public float velocidad = 500f;

    /// <summary>Rota el objeto cada frame, de forma independiente del framerate.</summary>
    void Update() {
        // Rotación continua sobre el eje X
        // Time.deltaTime asegura rotación independiente del framerate
        transform.Rotate(velocidad * Time.deltaTime, 0, 0);
    }
}