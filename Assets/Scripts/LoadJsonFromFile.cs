using UnityEngine;
using System.IO;

public class LoadJsonFromFile : MonoBehaviour
{
    public string customPath = "YourFolder/yourfile.json";
    public bool useCustomDataPath = true;
    void Start()
    {
        string filePath = Application.dataPath + "/YourFolder/yourfile.json";
        if(useCustomDataPath)
            filePath = customPath;
        
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            // var data = JsonUtility.FromJson<YourModelClass>(json);
            // or
            // var data = JsonConvert.DeserializeObject<YourModelClass>(json);

            // Access the data as needed
            Debug.Log(json);
        }
        else
        {
            Debug.LogError("JSON file not found at path: " + filePath);
        }
    }
}