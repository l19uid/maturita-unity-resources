using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class ReadBytesFromWebsite : MonoBehaviour
    {
        public string apiUrl = "https://maturita.delta-www.cz/prakticka/2023-zadani/squares.dat";
        
        void Start()
        {
            StartCoroutine(LoadBytesFromApi(apiUrl));
        }

        public IEnumerator LoadBytesFromApi(string link)
        {
            UnityWebRequest www = UnityWebRequest.Get(link);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load file: " + www.error);
                yield break;
            }

            byte[] bytes = www.downloadHandler.data;
            
            foreach (byte b in bytes)
            {
                string binaryString = System.Convert.ToString(b, 2).PadLeft(8, '0');
                
                // generalUseString += binaryString;
                
                Debug.Log(binaryString);
            }
        }
    }
}