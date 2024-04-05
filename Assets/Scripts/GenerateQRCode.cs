using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class GenerateQRCode : MonoBehaviour
    {
        //  Prefabs
        public GameObject whiteSquarePrefab;
        public GameObject blackSquarePrefab;
        public int squaresPerRow = 35;
        public float squareSize = 10f;
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
            int row = 0;
            int column = 0;
            foreach (byte b in bytes)
            {
                string binaryString = System.Convert.ToString(b, 2).PadLeft(8, '0');

                foreach (char bit in binaryString)
                {
                    // Instantiates the visual representation of the bit
                    GameObject squarePrefab = bit == '0' ? whiteSquarePrefab : blackSquarePrefab;
                    GameObject square = Instantiate(squarePrefab, transform);

                    square.transform.position = new Vector3(column * squareSize, -row * squareSize, 0f);

                    column++;

                    if (column >= squaresPerRow)
                    {
                        column = 0;
                        row++;
                    }
                }
            }
        }
    }
}