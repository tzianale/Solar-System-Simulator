using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using utils;

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

    public void SetPlanetInfo(Sprite inputSprite, string inputName, GameObject planetModel, CameraControl cameraCtrl, GameObject linkedInfoTab, Wrapper<GameObject> referenceToActiveTab, Button linkedCloseButton)
    {
        planetSprite.GetComponent<Image>().sprite = inputSprite;
        planetName.GetComponent<TextMeshProUGUI>().text = inputName;

        _planetInfoTab = linkedInfoTab;
        _planet3DObject = planetModel;

        cameraControl = cameraCtrl;
        _currentlyActiveTab = referenceToActiveTab;

        linkedCloseButton.onClick.AddListener(CloseThisTab);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.clickCount)
        {
            case 1:
                Debug.Log("Planet " + planetName.GetComponent<TextMeshProUGUI>().text + " clicked");

                if(CloseCurrentlyOpenTab()) break;
                
                _planetInfoTab.SetActive(true);
                
                _currentlyActiveTab.SetValue(_planetInfoTab);
                
                break;
            case 2: 
                Debug.Log("Planet " + planetName.GetComponent<TextMeshProUGUI>().text + " double clicked");

                if (cameraControl.getFollowingStatus())
                {
                    cameraControl.StopFollowing();
                }
                else
                {
                    cameraControl.SetToFollowPosition(_planet3DObject.transform);
                }
                break;
        }
    }

    private void CloseThisTab()
    {
        if (_currentlyActiveTab.GetValue() == _planetInfoTab)
        {
            CloseTab(_planetInfoTab);
        } 
    }

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
    /// <returns>
    /// A boolean, telling the caller if the closed tab was its own planet tab (true), or either another
    /// planet tab or no tab was closed (false)
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
