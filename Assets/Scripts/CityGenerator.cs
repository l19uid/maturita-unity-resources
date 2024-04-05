using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class CityGenerator : MonoBehaviour
{
    public float townBounds = 1000;
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
        float maxX = 0;
        float maxY = 0;

        for (int i = 0; i < data.towns.Count; i++)
        {
            if(data.towns[i].x > maxX)
                maxX = data.towns[i].x;
            if(data.towns[i].y > maxY)
                maxY = data.towns[i].y;
        }
        
        float gapX = townBounds / maxX;
        float gapY = townBounds / maxY;
        
        for (int i = 0; i < data.towns.Count; i++)
        {
            Vector3 position = new Vector3(data.towns[i].x * gapX, data.towns[i].y * gapY);
            
            GameObject town = Instantiate(townPrefab, position, Quaternion.identity, townParent.transform);
            town.name = data.towns[i].name;
            town.GetComponent<CityHover>().InitData(data.towns[i].name,data.towns[i].inhabitants ,data.towns[i].id);
            
            float newScale = data.towns[i].inhabitants / 500f;
            town.transform.localScale = new Vector3(newScale, newScale, newScale);
            towns.Add(town);
        }

        GenerateRoads();
    }

    private void GenerateRoads()
    {
        for (int i = 0; i < data.roads.Count; i++)
        {
            GameObject start = towns[data.roads[i].start-1];
            GameObject end = towns[data.roads[i].end-1];
            
            GameObject road = Instantiate(roadPrefab, start.transform.position, Quaternion.identity, roadParent.transform);
            road.name = start.name + " - " + end.name;
            road.GetComponent<LineRenderer>().positionCount = 2;
            road.GetComponent<LineRenderer>().SetPosition(0, start.transform.position);
            road.GetComponent<LineRenderer>().SetPosition(1, end.transform.position);
            roads.Add(road);
        }
    }
}
