using UnityEngine;

public class GiroVentilador : MonoBehaviour
{
    // Velocidad de rotación del ventilador en grados por segundo
    public float velocidad = 500f;

    void Update() {
        // Rotación continua en el eje Z, Time.deltaTime hace que la rotación sea independiente del framerate
        transform.Rotate(0, 0, velocidad * Time.deltaTime);
    }
}