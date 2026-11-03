using UnityEngine;

public class GiroEstrellas : MonoBehaviour
{
    // Velocidad de rotación en grados por segundo
    public float velocidad = 500f;

    void Update() {
        // Rotación continua sobre el eje Z
        // Time.deltaTime garantiza independencia del framerate
        transform.Rotate(0, 0, Time.deltaTime * velocidad);
    }
}