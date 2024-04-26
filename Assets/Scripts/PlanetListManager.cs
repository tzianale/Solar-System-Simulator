using System.Collections.Generic;
using UnityEngine;
using utils;
using Button = UnityEngine.UI.Button;

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

    private Wrapper<GameObject> _activeInfoTab = new (null);

    
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

        var planetInfoCloseButton = planetInfoTab.GetComponentInChildren<Button>();
        
        var propertyNames = new [] { "Mass", "Radius", "Surface Temperature"};
        var propertyValues = new [] { "CHONKY", "LONG", "HOT"};
        
        var planetDescription = 
            "This planet be planeting!" +
            "This planet be planeting!" + 
            "This planet be planeting!" +
            "This planet be planeting!" +
            "This planet be planeting!" +
            "This planet be planeting!" +
            "This planet be planeting!" +
            "This planet be planeting!" +
            "This planet be planeting!";
        
        Debug.Log(cameraControl);
        
        planetListElementPrefabController.SetPlanetInfo(planetSprite, planetName, planetObject, 
            cameraControl, planetInfoTab, _activeInfoTab, planetInfoCloseButton);
        
        planetInfoPrefabController.SetPlanetInfo(planetName, planetSprite,
            propertyNames, propertyValues, planetObject, sun, planetDescription);
        
        planetInfoTab.SetActive(false);
    }
}