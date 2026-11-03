using UnityEngine;

public class GiroMusicBox : MonoBehaviour
{
    // Velocidad de rotación en grados por segundo
    public float velocidad = 500f;

    void Update() {
        // Rotación continua sobre el eje X
        // Time.deltaTime asegura rotación independiente del framerate
        transform.Rotate(velocidad * Time.deltaTime, 0, 0);
    }
}