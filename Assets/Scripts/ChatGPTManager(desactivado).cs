using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class ChatGPTManager : MonoBehaviour
{
    [Header("Configuración")]
    public string apiKey; 
    private string endpoint = "https://api.together.xyz/v1/chat/completions";

    [System.Serializable]public class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]public class ChatRequest
    {
        public string model = "trinity-mini";
        public Message[] messages;
    }

    [System.Serializable]public class Choice
    {
        public Message message;
    }

    [System.Serializable]public class ChatResponse
    {
        public Choice[] choices;
    }

    public IEnumerator ObtenerRespuesta(string prompt, System.Action<string> callback)
    {
        ChatRequest requestData = new ChatRequest
        {
            messages = new Message[]
            {
                new Message { role = "user", content = prompt }
            }
        };

        string json = JsonConvert.SerializeObject(requestData);

        using (UnityWebRequest request = new UnityWebRequest(endpoint, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error API: " + request.error);
                callback?.Invoke("Error");
            }
            else
            {
                ChatResponse response = JsonConvert.DeserializeObject<ChatResponse>(request.downloadHandler.text);
                string respuesta = response.choices[0].message.content.Trim();
                callback?.Invoke(respuesta);
            }
        }
    }
}