using UnityEngine;

/// <summary>Hace parpadear la luz del objeto a intervalos aleatorios.</summary>
public class ParpadeoLuz : MonoBehaviour
{
    /// <summary>Componente <c>Light</c> del mismo objeto.</summary>
    private Light _light;

    /// <summary>Tiempo mínimo entre cambios de estado, en segundos.</summary>
    public float minTime = 0.05f;

    /// <summary>Tiempo máximo entre cambios de estado, en segundos.</summary>
    public float maxTime = 0.2f;

    /// <summary>Obtiene la luz del objeto e inicia la corrutina de parpadeo.</summary>
    void Start() {
        // Obtiene el componente Light asociado al objeto
        _light = GetComponent<Light>();

        // Inicia el sistema de parpadeo en coroutine independiente del Update
        StartCoroutine(Flicker());
    }

    /// <summary>Alterna el estado de la luz en bucle, con una espera aleatoria entre cambios.</summary>
    System.Collections.IEnumerator Flicker() {
        // Bucle infinito controlado por coroutine
        // El parpadeo solo se detiene si se desactiva o destruye el objeto
        while (true) {
            // Alterna el estado de la luz (efecto flicker on/off)
            _light.enabled = !_light.enabled;

            // Espera un tiempo aleatorio para simular inestabilidad eléctrica
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }
    }
}