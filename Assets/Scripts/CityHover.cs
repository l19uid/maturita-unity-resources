using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CityHover : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject cityInfoPanel;
    public GameObject cityInfoTitle;
    public GameObject cityInfoInhabitants;
    
    
    private string title;
    private int inhabitants;
    private int id;

    public void InitData(string title, int inhabitants, int id)
    {
        this.title = title;
        this.inhabitants = inhabitants;
        this.id = id;
    }
    
    void OnMouseOver()
    {
        cityInfoTitle.GetComponent<TMPro.TextMeshPro>().text = id + ". " + title;
        cityInfoInhabitants.GetComponent<TMPro.TextMeshPro>().text = inhabitants + " obyv.";
        cityInfoPanel.SetActive(true);
        cityInfoPanel.transform.position = transform.position + Vector3.up * 60;
    }

    void OnMouseExit()
    {
        Debug.Log("Mouse exit");
        cityInfoPanel.SetActive(false);
    }
}
