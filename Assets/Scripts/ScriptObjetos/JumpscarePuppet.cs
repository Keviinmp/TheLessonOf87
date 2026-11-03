using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.SceneManagement;

public class JumpscarePuppet : MonoBehaviour
{
    // Música ambiente que se detiene al iniciar el evento
    public AudioSource MusicaBox;

    // Audio previo al diálogo y voz principal del Puppet
    public AudioSource audioInicio;
    public AudioSource vozPuppet;

    // Scripts externos que deben quedar deshabilitados durante la secuencia
    public MonoBehaviour[] scriptsRotacion;

    // Componentes del vídeo final
    public VideoPlayer videoPlayer;
    public GameObject pantallaVideo;

    [Header("Grupo 1")]

    // Primer conjunto visual que parpadea sincronizado
    public Light[] ledsGrupo1;
    public GameObject objeto1;

    [Header("Grupo 2")]

    // Segundo conjunto visual independiente del primero
    public Light[] ledsGrupo2;
    public GameObject objeto2;

    // Evita volver a ejecutar la secuencia
    private bool activado;

    // Estado del jugador respecto al trigger
    private bool enZona;

    void Start()
    {
        // Ejecuta el método videoAcabado cuando acaba el video del jumpscare
        if (videoPlayer != null)
            videoPlayer.loopPointReached += VideoAcabado;
    }

    void Update() {
        // Sale inmediatamente si el evento ya está ejecutándose
        if (activado) 
            return;

        // Activa la secuencia solo si pulsas la E estando dentro del trigger
        if (enZona && Input.GetKeyDown(KeyCode.E)) {
            StartCoroutine(Cutscene());
        }
    }
    void OnTriggerEnter(Collider other) {
        // Marca disponibilidad de interacción
        if (other.CompareTag("Player")) 
            enZona = true;
    }

    void OnTriggerExit(Collider other) {
        // Cancela interacción fuera del área
        if (other.CompareTag("Player"))
            enZona = false;
    }

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

    // Grupo 1 mantiene sincronía entre luces y objeto asociado
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

    // Grupo 2 ejecuta el mismo sistema pero desacoplado temporalmente
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

    // Método que activa el sistema de vídeo del jumpscare
    void PlayVideo() {
        Debug.Log("PLAY VIDEO EJECUTADO");

        // Habilita superficie de render antes de reproducir
        if (pantallaVideo != null)
            pantallaVideo.SetActive(true);

        // Comienza reproducción del vídeo
        if (videoPlayer != null)
            videoPlayer.Play();
    }

    // Método jecutado automáticamente al finalizar el vídeo
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