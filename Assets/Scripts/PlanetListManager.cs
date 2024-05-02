using utils;
using System;
using UnityEngine;
using System.Collections.Generic;

public class PlanetListManager : MonoBehaviour
{
    [SerializeField]
    private GameObject sun;
    
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

    private readonly Wrapper<GameObject> _activeInfoTab = new (null);

    
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

        var planetInfoCloseButton = planetInfoPrefabController.CloseTabButton;

        Debug.Log(planetInfoCloseButton.name);
        
        var staticProperties = new Dictionary<string, string>()
        {
            {"Mass", "-Insert Mass Info Here-"},
            {"Orbit Duration", "One " + planetName + " year"}
        };

        var variableProperties = new Dictionary<string, Func<string>>()
        {
            {"Current Speed", () => planetObject.GetComponent<CelestialBody>().velocity.magnitude.ToString("n2")},
            {"Distance to Sun", () => (planetObject.transform.position - sun.transform.position).magnitude.ToString("n2")}
        };
        
        var planetDescription = 
            "The planet " + planetName + 
            " is a famous planet located in the Solar System Sol 12384905330, Galaxy Milky Way 49858456204852076205428, Universe 35904";
        
        Debug.Log(cameraControl);
        
        planetListElementPrefabController.SetPlanetInfo(planetSprite, planetName, planetObject, 
            cameraControl, planetInfoTab, _activeInfoTab, planetInfoCloseButton);
        
        planetInfoPrefabController.SetPlanetInfo(planetName, planetSprite,
            staticProperties, variableProperties, planetDescription);
        
        planetInfoTab.SetActive(false);
    }
}