using System.Collections.Generic;
using UnityEngine;

public class PlanetListManager : MonoBehaviour
{
    [SerializeField]
    private Transform planetListContent;
    
    [SerializeField]
    private Transform canvas;

    [SerializeField]
    private GameObject planetObjectPrefab;
    
    [SerializeField]
    private GameObject planetInfoPrefab;

    [SerializeField]
    private List<string> planetNames;

    [SerializeField]
    private List<Sprite> planetSprites;

    [SerializeField]
    private List<GameObject> planetModels;

    [SerializeField]
    private CameraControl cameraControl;

    
    // Start is called before the first frame update
    private void Start()
    {
        if (planetSprites.Count != planetNames.Count)
        {
            
        }
        else
        {
            for (var i = 0; i < planetNames.Count; i++)
            {
                CreateNewPlanet(planetSprites[i], planetNames[i], planetModels[i]);
            }
        }
    }

    private void CreateNewPlanet(Sprite planetSprite, string planetName, GameObject planetObject)
    {
        var planetListElement = Instantiate(planetObjectPrefab, planetListContent);
        var planetInfoTab = Instantiate(planetInfoPrefab, canvas);
        
        var planetListElementPrefabController = planetListElement.GetComponent<PlanetListElementPrefabController>();
        var planetInfoPrefabController = planetInfoTab.GetComponent<PlanetInfoPrefabController>();
        
        var propertyNames = new [] { "Temperature", "Size" };
        var propertyValues = new [] { "HOT", "BIG" };
        
        Debug.Log(cameraControl);
        
        planetListElementPrefabController.SetPlanetInfo(planetSprite, planetName, planetObject, cameraControl, planetInfoTab);
        planetInfoPrefabController.SetPlanetInfo(planetName, planetSprite, propertyNames, propertyValues);
        
        planetInfoTab.SetActive(false);
    }
}