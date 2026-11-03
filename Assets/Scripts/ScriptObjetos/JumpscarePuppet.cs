using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Secuencia del Puppet: al pulsar E en su zona se apaga la música, parpadean luces y objetos, suena su voz y se reproduce un vídeo final, tras el que se vuelve al menú.
/// </summary>
public class JumpscarePuppet : MonoBehaviour
{
    /// <summary>Música ambiente que se detiene al iniciar el evento.</summary>
    public AudioSource MusicaBox;

    /// <summary>Audio previo a la voz del Puppet.</summary>
    public AudioSource audioInicio;
    /// <summary>Voz principal del Puppet.</summary>
    public AudioSource vozPuppet;

    /// <summary>Scripts que se deshabilitan durante la secuencia (por ejemplo, los de rotación).</summary>
    public MonoBehaviour[] scriptsRotacion;

    /// <summary>Reproductor del vídeo final.</summary>
    public VideoPlayer videoPlayer;
    /// <summary>Superficie donde se reproduce el vídeo.</summary>
    public GameObject pantallaVideo;

    /// <summary>Luces del grupo 1, que parpadean sincronizadas con su objeto.</summary>
    [Header("Grupo 1")]

    public Light[] ledsGrupo1;
    /// <summary>Objeto del grupo 1 que se activa y desactiva junto a sus luces.</summary>
    public GameObject objeto1;

    /// <summary>Luces del grupo 2, que parpadean de forma independiente al grupo 1.</summary>
    [Header("Grupo 2")]

    public Light[] ledsGrupo2;
    /// <summary>Objeto del grupo 2 que se activa y desactiva junto a sus luces.</summary>
    public GameObject objeto2;

    /// <summary>Evita ejecutar la secuencia más de una vez.</summary>
    private bool activado;

    /// <summary>Indica si el jugador está dentro del trigger.</summary>
    private bool enZona;

    /// <summary>Se suscribe al evento de fin del vídeo.</summary>
    void Start()
    {
        // Ejecuta el método videoAcabado cuando acaba el video del jumpscare
        if (videoPlayer != null)
            videoPlayer.loopPointReached += VideoAcabado;
    }

    /// <summary>
    /// Lanza la secuencia si el jugador pulsa E dentro del trigger y aún no se ha ejecutado.
    /// </summary>
    void Update() {
        // Sale inmediatamente si el evento ya está ejecutándose
        if (activado) 
            return;

        // Activa la secuencia solo si pulsas la E estando dentro del trigger
        if (enZona && Input.GetKeyDown(KeyCode.E)) {
            StartCoroutine(Cutscene());
        }
    }

    /// <summary>
    /// Marca que el jugador puede interactuar.
    /// </summary>
    /// <param name="other">Collider que entra en el trigger.</param>
    void OnTriggerEnter(Collider other) {
        // Marca disponibilidad de interacción
        if (other.CompareTag("Player")) 
            enZona = true;
    }

    /// <summary>
    /// Cancela la posibilidad de interactuar al salir del área.
    /// </summary>
    /// <param name="other">Collider que sale del trigger.</param>
    void OnTriggerExit(Collider other) {
        // Cancela interacción fuera del área
        if (other.CompareTag("Player"))
            enZona = false;
    }

    /// <summary>
    /// Secuencia principal: detiene la música, desactiva scripts, inicia el parpadeo y las voces, y reproduce el vídeo cuando termina la voz.
    /// </summary>
    IEnumerator Cutscene() {
        // Bloquea futuras ejecuciones
        activado = true;

        // Elimina sonido ambiente para dejar prioridad al evento
        if (MusicaBox != null)
            MusicaBox.Stop();

            // Desactiva scripts que puedan modificar transformaciones
        if (scriptsRotacion != null) {
            foreach (var script in scriptsRotacion)
                if (script != null)
                    script.enabled = false;
        }

            yield return new WaitForSeconds(2f);

        // Ejecuta ambos sistemas visuales en paralelo
        StartCoroutine(ParpadeoGrupo1());
        StartCoroutine(ParpadeoGrupo2());

        // Reproduce introducción
        if (audioInicio != null)
            audioInicio.Play();

        yield return new WaitForSeconds(3f);

        // Inicia voz principal
        if (vozPuppet != null)
            vozPuppet.Play();

        // Espera duración real del clip antes de continuar
        yield return new WaitForSeconds(vozPuppet.clip.length);

        PlayVideo();
    }

    /// <summary>Parpadeo aleatorio del grupo 1: luces y objeto comparten el mismo estado.</summary>
    IEnumerator ParpadeoGrupo1() {
        while (activado)
        {
            // Calcula estado aleatorio por iteración
            bool estado = Random.value > 0.5f;

            if (ledsGrupo1 != null) {
                foreach (Light led in ledsGrupo1)
                    if (led != null)
                        led.enabled = estado;
            }

            // Replica el mismo estado visual sobre el objeto
            if (objeto1 != null)
                objeto1.SetActive(estado);

            // Intervalo aleatorio para evitar patrón repetitivo
            yield return new WaitForSeconds(Random.Range(0.04f, 0.2f));
        }
    }

    /// <summary>Parpadeo aleatorio del grupo 2, independiente del primero.</summary>
    IEnumerator ParpadeoGrupo2() {
        while (activado) {
            bool estado = Random.value > 0.5f;

            if (ledsGrupo2 != null) {
                foreach (Light led in ledsGrupo2)
                    if (led != null)
                        led.enabled = estado;
            }

            if (objeto2 != null)
                objeto2.SetActive(estado);

            yield return new WaitForSeconds(Random.Range(0.04f, 0.2f));
        }
    }

    /// <summary>Muestra la pantalla de vídeo y reproduce el jumpscare.</summary>
    void PlayVideo() {
        Debug.Log("PLAY VIDEO EJECUTADO");

        // Habilita superficie de render antes de reproducir
        if (pantallaVideo != null)
            pantallaVideo.SetActive(true);

        // Comienza reproducción del vídeo
        if (videoPlayer != null)
            videoPlayer.Play();
    }

    /// <summary>
    /// Se ejecuta al acabar el vídeo: lo detiene, oculta la pantalla, libera el cursor y vuelve al menú.
    /// </summary>
    /// <param name="vp">Reproductor que ha terminado.</param>
    void VideoAcabado(VideoPlayer vp) {
        // Limpieza del reproductor
        if (videoPlayer != null)
            videoPlayer.Stop();

        // Oculta pantalla de render
        if (pantallaVideo != null)
            pantallaVideo.SetActive(false);

        // Restaura control del usuario
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Reinicia flujo cargando menú
        SceneManager.LoadScene("MenuInicio");
    }
}