using UnityEngine;

public class UIManager : MonoBehaviour
{
    public DialogoUI dialogoUI;            
    public PreguntasManager preguntasManager;

    public void MostrarPregunta()
{
    if (preguntasManager == null)
    {
        Debug.LogError("❌ PreguntasManager no asignado en UIManager");
        return;
    }

    if (dialogoUI == null)
    {
        Debug.LogError("❌ DialogoUI no asignado en UIManager");
        return;
    }

    var pregunta = preguntasManager.GetPreguntaRandom();

    if (pregunta == null)
    {
        Debug.LogError("❌ No hay preguntas cargadas (JSON no inicializado)");
        return;
    }

    dialogoUI.Abrir(pregunta.pregunta, pregunta.respuesta);
}
}
