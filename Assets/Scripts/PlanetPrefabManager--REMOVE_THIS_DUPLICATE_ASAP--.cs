/*
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the Planet List Item Prefabs.
/// Class has two methods, one to set up the Prefab with the correct information and one to react to clicks
/// </summary>
public class PlanetPrefabManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject planetSprite;

    [SerializeField]
    private GameObject planetName;

    [SerializeField] 
    private GameObject planet3DObject;

    [SerializeField]
    private CameraControl cameraControl;

    public GameObject Planet3DObject => planet3DObject;
    public CameraControl CameraControl => cameraControl;

    /// <summary>
    /// Constructor-like method, sets up the prefab with the correct information
    /// </summary>
    /// 
    /// <param name="inputSprite">
    /// The Image that will be associated to this planet
    /// </param>
    /// 
    /// <param name="inputName">
    /// The name of this planet
    /// </param>
    /// 
    /// <param name="planetModel">
    /// The GameObject that represents the planet in the simulation
    /// </param>
    /// 
    /// <param name="cameraCtrl">
    /// The CameraControl script.
    /// This allows the program to focus the camera on the planet if a double click is detected
    /// </param>
    public void SetPlanetInfo(Sprite inputSprite, string inputName, GameObject planetModel, CameraControl cameraCtrl)

    {
        planetSprite.GetComponent<Image>().sprite = inputSprite;
        planetName.GetComponent<TextMeshProUGUI>().text = inputName;
        
        planet3DObject = planetModel;

        cameraControl = cameraCtrl;

    }

    /// <summary>
    /// Click handler, detects if the cursor was pressed while hovering over the Planet List Item.
    ///
    /// If a single click is detected, the info tab will be opened / closed
    /// If a double click is detected, the camera will focus on the selected planet
    /// </summary>
    /// 
    /// <param name="eventData">
    /// The object storing the data about the click event
    /// </param>
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.clickCount)
        {
            case 1:
                Debug.Log("Planet " + planetName.GetComponent<TextMeshProUGUI>().text + " clicked");
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
*/