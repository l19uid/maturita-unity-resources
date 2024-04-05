using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ApiToJson : MonoBehaviour
{
    
    public string apiUrl = "https://api.example.com/data";
    void Start()
    {
        StartCoroutine(LoadJsonFromApi(apiUrl));
    }

    public IEnumerator LoadJsonFromApi(string link)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(link))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string json = webRequest.downloadHandler.text;

                // var data = JsonUtility.FromJson<CustoModelClass>(json);
                // or
                // var data = JsonConvert.DeserializeObject<CustoModelClass>(json);

                Debug.Log(json);
            }
        }
    }
}