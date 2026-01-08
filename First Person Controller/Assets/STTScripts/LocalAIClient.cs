using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

public class LocalAIClient : MonoBehaviour
{
    [SerializeField] private string apiUrl = "http://localhost:11434/api/chat";
    [SerializeField] private string modelName = "gemma2:latest";

    /*public async Task<string> CallLocalAIAsync(string userText)
    {
        GenerateRequest req = new GenerateRequest
        {
            model = modelName,
            prompt = "Ets un NPC del joc. Respon curt: " + userText,
            stream = false
        };

        string json = JsonUtility.ToJson(req);
        Debug.Log("Enviant a IA: " + json);

        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            var op = www.SendWebRequest();
            while (!op.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error IA: " + www.error + " | " + www.downloadHandler.text);
                return "Error de connexió";
            }

            Debug.Log("Resposta crua: " + www.downloadHandler.text);

            string response = ParseAIResponse(www.downloadHandler.text);
            Debug.Log("IA respon: " + response);
            return response;
        }
    }

    private string ParseAIResponse(string jsonResponse)
    {
        if (jsonResponse.Contains("\"response\":"))
        {
            int start = jsonResponse.IndexOf("\"response\":\"") + 12;
            int end = jsonResponse.IndexOf("\"", start);
            if (start > 11 && end > start)
            {
                return jsonResponse.Substring(start, end - start)
                    .Replace("\\n", "\n")
                    .Replace("\\\"", "\"");
            }
        }
        return "No puc entendre la resposta";
    }*/

    public async Task<string> CallLocalAIAsync(string userText)
    {
        ChatRequest req = new ChatRequest
        {
            model = modelName,
            stream = false,
            messages = new[]
            {
                new ChatMessage { role = "system", content = "Ets un NPC del meu joc i respons curt i clar." },
                new ChatMessage { role = "user", content = userText }
            }
        };

        string json = JsonUtility.ToJson(req);
        Debug.Log("Enviant a IA: " + json);

        using(UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            var op = www.SendWebRequest();
            while (!op.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error IA: " + www.error + " | " + www.downloadHandler.text);
                return "Error de connexió";
            }

            Debug.Log("Resposta crua: " + www.downloadHandler.text);

            ChatResponse resp = JsonUtility.FromJson<ChatResponse>(www.downloadHandler.text);
            string resposta = resp?.message?.content ?? "Resposta buida";

            return resposta;
        }
    }
}
