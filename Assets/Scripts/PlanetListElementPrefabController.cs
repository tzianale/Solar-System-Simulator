using utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Controller for the Prefabs that will be used as Elements in the Planet List
/// </summary>
public class PlanetListElementPrefabController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject planetSprite;

    [SerializeField]
    private GameObject planetName;

    [SerializeField]
    private CameraControl cameraControl;
    

    private GameObject _planetInfoTab;
    private GameObject _planet3DObject;
    
    private Wrapper<GameObject> _currentlyActiveTab;

    private bool actdel = true;

    /// <summary>
    /// Constructor-like method, sets all the relevant information and references, as well as linking the Closing Button
    /// to the Close Tab method, allowing the script to work correctly
    /// </summary>
    /// 
    /// <param name="inputSprite">
    /// Reference to the element where the planet icon will be placed
    /// </param>
    /// 
    /// <param name="inputName">
    /// The name of the planet
    /// </param>
    /// 
    /// <param name="planetModel">
    /// Reference to the Sphere Object in the simulation which is representing this planet
    /// </param>
    /// 
    /// <param name="cameraCtrl">
    /// Reference to the Camera Controller Script for this simulation
    /// </param>
    /// 
    /// <param name="linkedInfoTab">
    /// The info tab with the properties of this planet
    /// </param>
    /// 
    /// <param name="referenceToActiveTab">
    /// Wrapper object containing continuously updated info about the info tab that is currently open
    /// </param>
    /// 
    /// <param name="linkedCloseButton">
    /// Reference to the button that, when pressed, should close the info tab
    /// </param>
    public void SetPlanetInfo(Sprite inputSprite, string inputName, GameObject planetModel, CameraControl cameraCtrl, 
        GameObject linkedInfoTab, Wrapper<GameObject> referenceToActiveTab, Button linkedCloseButton)
    {
        planetSprite.GetComponent<Image>().sprite = inputSprite;
        planetName.GetComponent<TextMeshProUGUI>().text = inputName;

        _planetInfoTab = linkedInfoTab;
        _planet3DObject = planetModel;

        cameraControl = cameraCtrl;
        _currentlyActiveTab = referenceToActiveTab;

        linkedCloseButton.onClick.AddListener(CloseThisTab);
    }

    /// <summary>
    /// Registers clicks to the Planet List Element.
    /// The number of clicks is then given to the HandleClickEvent function
    /// </summary>
    /// 
    /// <param name="eventData">
    /// The object storing various information about the click event
    /// </param>
    public void OnPointerClick(PointerEventData eventData)
    {
        HandleClickEvent(eventData.clickCount);
    }

    /// <summary>
    /// Handles the events following a click on a planet:
    /// One click should open the info tab,
    /// Two clicks should tell the camera to focus on the 3D object corresponding to this specific planet,
    /// Three clicks should hide / delete the planet (TODO change commentary)
    /// </summary>
    /// 
    /// <param name="clickCount">
    /// How many times the button was clicked
    /// </param>
    public void HandleClickEvent(int clickCount)
    {
        switch (clickCount)
        {
            case 1:
                Debug.Log("Planet " + planetName.GetComponent<TextMeshProUGUI>().text + " single clicked");

                if(CloseCurrentlyOpenTab()) break;
                
                _planetInfoTab.SetActive(true);
                
                _currentlyActiveTab.SetValue(_planetInfoTab);
                
                break;
            case 2: 
                Debug.Log("Planet " + planetName.GetComponent<TextMeshProUGUI>().text + " double clicked");

                if (cameraControl.GetFollowingTarget() != null && cameraControl.GetFollowingTarget().Equals(_planet3DObject.transform))
                {
                    cameraControl.StopFollowing();
                }
                else
                {
                    cameraControl.SetToFollowPosition(_planet3DObject.transform);
                }
                break;
            case 3:
                Debug.Log("Planet " + planetName.GetComponent<TextMeshProUGUI>().text + " first clicked");
                actdel = !actdel;
                _planet3DObject.SetActive(actdel);
                break;
        }
    }

    /// <summary>
    /// When called checks if the currently active tab corresponds to the planet of the Element.
    /// If so, the tab will be closed, else nothing will happen
    /// </summary>
    private void CloseThisTab()
    {
        if (_currentlyActiveTab.GetValue() == _planetInfoTab)
        {
            CloseTab(_planetInfoTab);
        } 
    }

    /// <summary>
    /// Closes the info tab provided in the parameters and sets the currently active tab Wrapper to null
    /// </summary>
    /// 
    /// <param name="tabToClose">
    /// The tab to be closed
    /// </param>
    private void CloseTab(GameObject tabToClose)
    {
        tabToClose.SetActive(false);

        if (_currentlyActiveTab.GetValue() == tabToClose)
        {
            _currentlyActiveTab.SetValue(null);
        }
    }

    /// <summary>
    /// Closes the tab that is currently open. If no tab is found, nothing happens.
    /// </summary>
    /// 
    /// <returns>
    /// Returns true if the closed tab was the caller's own planet tab.
    /// Otherwise, returns false if another planet tab or no tab was closed.
    /// </returns>
    private bool CloseCurrentlyOpenTab()
    {
        if (_currentlyActiveTab.GetValue() == _planetInfoTab)
        {
            CloseTab(_planetInfoTab);
            return true;
        } 
        
        if (_currentlyActiveTab.GetValue() != null)
        {
            CloseTab(_currentlyActiveTab.GetValue());
        }

        return false;
    }
}
