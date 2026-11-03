using UnityEngine;

public class CogerMascara : MonoBehaviour
{
    [Header("Objeto equipado")]
    public GameObject mascaraEnJugador;

    [Header("Vida que da")]
    public int vidasQueDa = 1;

    private bool enZona;
    private bool recogido;

    void Update()
    {
        if (recogido)
            return;

        // Igual que el Puppet → E dentro del trigger
        if (enZona && Input.GetKeyDown(KeyCode.E))
        {
            RecogerMascara();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            enZona = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            enZona = false;
    }

    void RecogerMascara()
    {
        recogido = true;

        // Sumar vida y actualizar UI
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SumarVida(vidasQueDa);
        }

        // Equipar máscara al jugador
        if (mascaraEnJugador != null)
        {
            mascaraEnJugador.SetActive(true);
        }

        // Ocultar máscara del suelo
        gameObject.SetActive(false);
    }
}