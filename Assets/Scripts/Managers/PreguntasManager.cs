using UnityEngine;

/// <summary>Pregunta con su respuesta correcta, tal y como se guarda en los ficheros JSON.</summary>
[System.Serializable] // Etiqueta para poder manejar los JSON
public class Pregunta {
    /// <summary>Enunciado mostrado al jugador.</summary>
    public string pregunta;

    /// <summary>Respuesta correcta asociada a la pregunta.</summary>
    public string respuesta;
}

/// <summary>
/// Carga los bancos de preguntas desde ficheros JSON y devuelve preguntas aleatorias del banco activo.
/// </summary>
public class PreguntasManager : MonoBehaviour {
    /// <summary>Preguntas del banco cargado actualmente en memoria.</summary>
    public Pregunta[] preguntas;

    /// <summary>Ficheros JSON de preguntas, uno por prueba del juego.</summary>
    public TextAsset[] jsonPorPrueba;


    /// <summary>Clase auxiliar necesaria para deserializar un array JSON con <c>JsonUtility</c>.</summary>
    [System.Serializable]
    private class PreguntaWrapper
    {
        /// <summary>Preguntas deserializadas.</summary>
        public Pregunta[] items;
    }


    /// <summary>
    /// Carga el banco de preguntas correspondiente al índice de prueba indicado.
    /// </summary>
    /// <param name="index">Índice de la prueba (empieza en 0).</param>
    public void CargarPorIndice(int index) {
        // Comprueba que existen archivos JSON asignados
        if (jsonPorPrueba == null || jsonPorPrueba.Length == 0) {
            Debug.LogError("No hay JSON asignados");
            return;
        }

        // Valida que el índice recibido exista dentro del array
        if (index < 0 || index >= jsonPorPrueba.Length) {
            Debug.LogError("Índice de prueba inválido: " + index);
            return;
        }

        // Convierte el contenido JSON en objetos Pregunta
        preguntas = JsonUtility.FromJson<PreguntaWrapper>(
            "{\"items\":" + jsonPorPrueba[index].text + "}"
        ).items;

        Debug.Log(
            "Preguntas cargadas para prueba: "
            + (index + 1)
            + " | Total: "
            + preguntas.Length
        );
    }


    /// <summary>
    /// Devuelve una pregunta aleatoria del banco cargado.
    /// </summary>
    /// <returns>Una <see cref="Pregunta"/> aleatoria, o <c>null</c> si no hay preguntas cargadas.</returns>
    public Pregunta GetPreguntaRandom() {
        // Evita acceder a preguntas inexistentes
        if (preguntas == null || preguntas.Length == 0) {
            Debug.LogError("No hay preguntas cargadas!");
            return null;
        }

        // Selecciona una posición aleatoria del array
        return preguntas[Random.Range(0, preguntas.Length)];
    }
}