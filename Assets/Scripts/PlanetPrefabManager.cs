using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlanetPrefabManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject planetSprite;

    [SerializeField]
    private GameObject planetName;

    [SerializeField] 
    public GameObject planet3DObject;

    [SerializeField] 
    public Camera linkedCamera;

    [SerializeField] 
    public int cameraAdjustmentOnPlanetClick;
    
    public void SetPlanetInfo(Sprite inputSprite, string inputName, GameObject planetModel, int cameraAdjustment, Camera gameCamera)
    {
        planetSprite.GetComponent<Image>().sprite = inputSprite;
        planetName.GetComponent<TextMeshProUGUI>().text = inputName;
        
        planet3DObject = planetModel;
        linkedCamera = gameCamera;

        cameraAdjustmentOnPlanetClick = cameraAdjustment;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.clickCount)
        {
            case 1:
                Debug.Log("Planet " + planetName.GetComponent<TextMeshProUGUI>().text + " clicked");
                break;
            case 2: 
                Debug.Log("Planet " + planetName.GetComponent<TextMeshProUGUI>().text + " double clicked");
            
                var newCameraPosition = planet3DObject.GetComponent<Transform>().position;

                newCameraPosition.y += cameraAdjustmentOnPlanetClick;
            
                linkedCamera.GetComponent<Transform>().position = newCameraPosition;
                
                break;
        }
    }
}
