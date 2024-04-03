using System.Collections.Generic;
using UnityEngine;

public class PlanetListManager : MonoBehaviour
{
    [SerializeField]
    private Transform planetListContent;

    [SerializeField]
    private GameObject planetPrefab;

    [SerializeField]
    private List<string> planetNames;

    [SerializeField]
    private List<Sprite> planetSprites;

    [SerializeField]
    private List<GameObject> planetModels;

    [SerializeField] private List<int> cameraAdjustments;

    [SerializeField]
    private Camera gameCamera;

    // Start is called before the first frame update
    private void Start()
    {
        if (planetSprites.Count != planetNames.Count)
        {
            
        }
        else
        {
            for (var planetIndex = 0; planetIndex < planetNames.Count; planetIndex++)
            {
                CreateNewPlanet(planetSprites[planetIndex], planetNames[planetIndex], planetModels[planetIndex], cameraAdjustments[planetIndex], gameCamera);
            }
        }
    }

    private void CreateNewPlanet(Sprite planetSprite, string planetName, GameObject planetObject, int cameraAdjustment, Camera cameraToLink)
    {
        var planet = Instantiate(planetPrefab, planetListContent);
        
        var planetCreator = planet.GetComponent<PlanetPrefabManager>();
        
        planetCreator.SetPlanetInfo(planetSprite, planetName, planetObject, cameraAdjustment, cameraToLink);
    }
}
