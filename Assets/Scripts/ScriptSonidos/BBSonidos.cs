using UnityEngine;
using System.Collections;

public class BBSonidos : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Audios")]
    public AudioClip[] sonidos; // mete aquí los 5 audios

    [Header("Tiempo")]
    public float intervalo = 20f;

    private void Start()
    {
        StartCoroutine(ReproducirAleatorio());
    }

    IEnumerator ReproducirAleatorio()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalo);

            if (sonidos == null || sonidos.Length == 0)
                continue;

            AudioClip elegido = sonidos[Random.Range(0, sonidos.Length)];

            audioSource.PlayOneShot(elegido);
        }
    }
}