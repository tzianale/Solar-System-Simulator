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
            for (int i = 0; i < planetNames.Count; i++)
            {
                CreateNewPlanet(planetSprites[i], planetNames[i], planetModels[i]);
            }
        }
    }

    private void CreateNewPlanet(Sprite planetSprite, string planetName, GameObject planetObject)
    {
        var planetInstance = Instantiate(planetPrefab, planetListContent);
        var planetManager = planetInstance.GetComponent<PlanetPrefabManager>();
        Debug.Log(cameraControl);
        planetManager.SetPlanetInfo(planetSprite, planetName, planetObject, cameraControl);
    }
}
