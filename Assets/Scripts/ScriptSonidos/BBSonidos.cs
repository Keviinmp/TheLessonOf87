using UnityEngine;
using System.Collections;

/// <summary>Reproduce un sonido aleatorio a intervalos regulares.</summary>
public class BBSonidos : MonoBehaviour {

    /// <summary>Fuente de audio desde la que se reproducen los clips.</summary>
    public AudioSource audioSource;

    /// <summary>Sonidos posibles; se elige uno al azar en cada reproducción.</summary>
    [Header("Audios")]

    public AudioClip[] sonidos;

    /// <summary>Segundos entre reproducciones.</summary>
    [Header("Tiempo")]

    public float intervalo = 20f;


    /// <summary>Inicia la rutina de reproducción.</summary>
    private void Start() {
        StartCoroutine(ReproducirAleatorio());
    }


    /// <summary>Corrutina que, en bucle, espera el intervalo y reproduce un clip aleatorio.</summary>
    IEnumerator ReproducirAleatorio() {

        while (true) {

            // Espera el intervalo configurado antes de reproducir el siguiente sonido
            yield return new WaitForSeconds(intervalo);

            // Validación de seguridad: evita errores si no hay sonidos asignados
            if (sonidos == null || sonidos.Length == 0)
                continue;

            // Selecciona un clip aleatorio del array
            AudioClip elegido = sonidos[Random.Range(0, sonidos.Length)];

            // Reproduce el clip sin interrumpir otros sonidos del AudioSource
            audioSource.PlayOneShot(elegido);
        }
    }
}