using UnityEngine;

/// <summary>
/// Permite recoger una máscara con la tecla E dentro de su área, lo que da vidas al jugador.
/// </summary>
public class CogerMascara : MonoBehaviour {

    /// <summary>Máscara que se activa en el jugador al recogerla.</summary>
    [Header("Objeto equipado")]

    public GameObject mascaraEnJugador;

    /// <summary>Vidas que otorga al recogerla.</summary>
    [Header("Vida que da")]

    public int vidasQueDa = 1;

    /// <summary>Indica si el jugador está dentro del área de interacción.</summary>
    private bool enZona;

    /// <summary>Evita recoger la máscara más de una vez.</summary>
    private bool recogido;


    /// <summary>Recoge la máscara si el jugador pulsa E dentro de la zona.</summary>
    void Update() {

        if (recogido)
            return;

        // Interacción tipo "E dentro del trigger"
        if (enZona && Input.GetKeyDown(KeyCode.E)) {
            RecogerMascara();
        }
    }


    /// <summary>
    /// Detecta la entrada del jugador en el área de interacción.
    /// </summary>
    /// <param name="other">Collider que entra en el trigger.</param>
    void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player"))
            enZona = true;
    }


    /// <summary>
    /// Detecta la salida del jugador del área de interacción.
    /// </summary>
    /// <param name="other">Collider que sale del trigger.</param>
    void OnTriggerExit(Collider other) {

        if (other.CompareTag("Player"))
            enZona = false;
    }


    /// <summary>Suma vidas, activa la máscara en el jugador y desactiva el objeto del suelo.</summary>
    void RecogerMascara() {

        // Marca el objeto como recogido para evitar reuso
        recogido = true;

        // Aumenta la vida del jugador a través del GameManager
        if (GameManager.Instance != null) {
            GameManager.Instance.SumarVida(vidasQueDa);
        }

        // Activa la máscara equipada en el jugador
        if (mascaraEnJugador != null) {
            mascaraEnJugador.SetActive(true);
        }

        // Desactiva el objeto del suelo tras ser recogido
        gameObject.SetActive(false);
    }
}