using UnityEngine;
using System.Collections;

public class LucesPruebasManager : MonoBehaviour
{
    public Animator luz1;
    public Animator luz2;
    public Animator luz3;
    public Animator luz4;

    [Header("Evento final")]
    public GameObject objetoFinal;
    public Light[] lucesFuera;

    [Header("Tiempo tras última luz")]
    public float esperaEventoFinal = 2f;

    private int lucesActivadas = 0;
    private bool eventoLanzado = false;

    private void Start()
    {
        // Opcional: apagar luces al inicio
        if (lucesFuera != null)
        {
            foreach (Light l in lucesFuera)
            {
                if (l != null)
                    l.enabled = false;
            }
        }
    }

    public void ActivarLuz(int numeroPrueba)
    {
        if (eventoLanzado) return;

        Animator luz = GetLuz(numeroPrueba);

        if (luz == null)
        {
            Debug.LogError("No hay Animator asignado para la luz " + numeroPrueba);
            return;
        }

        luz.SetTrigger("Activar");

        lucesActivadas++;

        Debug.Log("Luz activada: " + numeroPrueba + " | Total: " + lucesActivadas);

        if (lucesActivadas >= 4)
        {
            eventoLanzado = true;
            StartCoroutine(EventoFinal());
        }
    }

    IEnumerator EventoFinal()
    {
        yield return new WaitForSeconds(esperaEventoFinal);

        Debug.Log("EVENTO FINAL");

        if (objetoFinal != null)
            objetoFinal.SetActive(false);

        if (lucesFuera != null)
        {
            foreach (Light l in lucesFuera)
            {
                if (l != null)
                    l.enabled = true;
            }
        }
    }

    private Animator GetLuz(int numeroPrueba)
    {
        switch (numeroPrueba)
        {
            case 1: return luz1;
            case 2: return luz2;
            case 3: return luz3;
            case 4: return luz4;
            default:
                Debug.LogError("Numero invalido");
                return null;
        }
    }
}