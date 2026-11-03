using UnityEngine;

[System.Serializable]
public class Pregunta
{
    public string pregunta;
    public string respuesta;
}

public class PreguntasManager : MonoBehaviour
{
    public Pregunta[] preguntas;

    // Array de JSON (uno por prueba)
    public TextAsset[] jsonPorPrueba;

    // Wrapper necesario para JsonUtility
    [System.Serializable]
    private class PreguntaWrapper
    {
        public Pregunta[] items;
    }

    // Cargar JSON según la prueba
    public void CargarPorIndice(int index)
    {
        if (jsonPorPrueba == null || jsonPorPrueba.Length == 0)
        {
            Debug.LogError("No hay JSON asignados");
            return;
        }

        if (index < 0 || index >= jsonPorPrueba.Length)
        {
            Debug.LogError("Índice de prueba inválido: " + index);
            return;
        }

        preguntas = JsonUtility.FromJson<PreguntaWrapper>(
            "{\"items\":" + jsonPorPrueba[index].text + "}"
        ).items;

        Debug.Log("Preguntas cargadas para prueba: " + (index+1)  +
                  " | Total: " + preguntas.Length);
    }

    // Pregunta aleatoria del JSON cargado
    public Pregunta GetPreguntaRandom()
    {
        if (preguntas == null || preguntas.Length == 0)
        {
            Debug.LogError("No hay preguntas cargadas!");
            return null;
        }

        return preguntas[Random.Range(0, preguntas.Length)];
    }
}