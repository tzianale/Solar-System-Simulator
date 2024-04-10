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
    private GameObject planet3DObject;

    [SerializeField] 
    private GameObject focalPoint;

    [SerializeField] 
    private Vector3 cameraAdjustmentOnPlanetClick;
    
    public void SetPlanetInfo(Sprite inputSprite, string inputName, GameObject planetModel, Vector3 cameraAdjustment, GameObject focalPoint)
    {
        planetSprite.GetComponent<Image>().sprite = inputSprite;
        planetName.GetComponent<TextMeshProUGUI>().text = inputName;
        
        planet3DObject = planetModel;
        this.focalPoint = focalPoint;

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

                var objectFollower = focalPoint.GetComponent<CameraControl>();
                
                objectFollower.SetFocusedObject(planet3DObject.transform);
                
                break;
        }
    }
}
