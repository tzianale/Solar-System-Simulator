using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlanetListElementPrefabController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject planetSprite;

    [SerializeField]
    private GameObject planetName;

    [SerializeField]
    private GameObject planetInfoTab;

    [SerializeField] 
    private GameObject planet3DObject;

    [SerializeField]
    private CameraControl cameraControl;
    
    private bool _tabActive;

    public void SetPlanetInfo(Sprite inputSprite, string inputName, GameObject planetModel, CameraControl cameraCtrl, GameObject linkedInfoTab)

    {
        planetSprite.GetComponent<Image>().sprite = inputSprite;
        planetName.GetComponent<TextMeshProUGUI>().text = inputName;

        planetInfoTab = linkedInfoTab;
        planet3DObject = planetModel;

        cameraControl = cameraCtrl;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.clickCount)
        {
            case 1:
                Debug.Log("Planet " + planetName.GetComponent<TextMeshProUGUI>().text + " clicked");
                
                planetInfoTab.SetActive(!_tabActive);
                _tabActive = !_tabActive;
                
                break;
            case 2: 
                Debug.Log("Planet " + planetName.GetComponent<TextMeshProUGUI>().text + " double clicked");

                if (cameraControl.getFollowingStatus())
                {
                    cameraControl.StopFollowing();
                }
                else
                {
                    cameraControl.SetToFollowPosition(planet3DObject.transform);
                }
                break;
        }
    }
}
