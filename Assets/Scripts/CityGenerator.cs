using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class CityGenerator : MonoBehaviour
{
    public Vector2 townBounds = new Vector2(0, 1000);
    public string json = "";
    public GameObject townPrefab;
    public GameObject roadPrefab;
    public GameObject townParent;
    public GameObject roadParent;

    private List<GameObject> towns = new List<GameObject>();
    private List<GameObject> roads = new List<GameObject>();
    
    private class Town
    {
        public int id { get; set; }
        public string name { get; set; }
        public int inhabitants { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
    
    private class Road
    {
        public int id { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public int travelTime { get; set; }
        public int upgradePrice { get; set; }
    }

    class RetrievedData
    {
        public List<Town> towns { get; set; }
        public List<Road> roads { get; set; }
    }

    private RetrievedData data;
 
    void Start()
    { 
        StartCoroutine(LoadJsonFromApi("https://maturita.delta-www.cz/prakticka/2023-map/mapData/"));
    }
    
    void LoadData()
    {
        data = JsonConvert.DeserializeObject<RetrievedData>(json);
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
                json = webRequest.downloadHandler.text;
                LoadData();
                GenerateTownObjects();
            }
        }
    }

    private void GenerateTownObjects()
    {
        for (int i = 0; i < data.towns.Count; i++)
        {
            Vector3 position = new Vector3(data.towns[i].x, data.towns[i].y);
            
            GameObject town = Instantiate(townPrefab, position, Quaternion.identity, townParent.transform);
            town.name = data.towns[i].name;
            
            float newScale = data.towns[i].inhabitants / 1000f;
            town.transform.localScale = new Vector3(newScale, newScale, newScale);
            towns.Add(town);
        }

        MakeCameraSeeAllTowns();
    }
    
    private void MakeCameraSeeAllTowns()
    {
        Bounds bounds = new Bounds(towns[0].transform.position, Vector3.zero);
        foreach (GameObject town in towns)
        {
            bounds.Encapsulate(town.transform.position);
        }
        Camera.main.transform.position = new Vector3(bounds.center.x, bounds.center.y, Camera.main.transform.position.z);
    }
}
