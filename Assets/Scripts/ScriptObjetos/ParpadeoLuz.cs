using UnityEngine;

public class ParpadeoLuz : MonoBehaviour
{
    // Referencia al componente Light del mismo GameObject
    private Light _light;

    // Tiempo mínimo entre cambios de estado del parpadeo
    public float minTime = 0.05f;

    // Tiempo máximo entre cambios de estado del parpadeo
    public float maxTime = 0.2f;

    void Start() {
        // Obtiene el componente Light asociado al objeto
        _light = GetComponent<Light>();

        // Inicia el sistema de parpadeo en coroutine independiente del Update
        StartCoroutine(Flicker());
    }

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