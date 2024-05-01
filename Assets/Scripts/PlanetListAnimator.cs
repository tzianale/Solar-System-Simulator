using TMPro;
using UnityEngine;
using System.Collections;

public class PlanetListContainerScript : MonoBehaviour
{
    private Transform _planetListContainerTransform;
    private bool _listIsOpen;
    
    private float moveDistance = 210f; // The distance to move the panel.
    private float moveSpeed = 5.0f; // How fast the panel moves.
    private TextMeshProUGUI buttonText;

    // Start is called before the first frame update
    void Start()
    {
        _planetListContainerTransform = transform;
    }

    public Transform PlanetListContainerTransform
    {
        get => _planetListContainerTransform;
        set => _planetListContainerTransform = value;
    }

    public bool ListIsOpen
    {
        get => _listIsOpen;
        private set => _listIsOpen = value;
    }
    
    public TextMeshProUGUI ButtonText
    {
        get => buttonText;
        set => buttonText = value;
    }

    public void OnArrowClick()
    {
    Debug.Log("OnArrowClick called. Current _listIsOpen: " + _listIsOpen);

    if (_listIsOpen)
    {
        StartCoroutine(MovePanel(Vector3.down * moveDistance));
        _listIsOpen = false;
        buttonText.text = "↑";
        Debug.Log("Panel is now closing. New text: " + buttonText.text);
    }
    else
    {
        StartCoroutine(MovePanel(Vector3.up * moveDistance));
        _listIsOpen = true;
        buttonText.text = "↓";
        Debug.Log("Panel is now opening. New text: " + buttonText.text);
    }
    }

    


    private IEnumerator MovePanel(Vector3 target)
    {
        if (_planetListContainerTransform == null)
    {
        Debug.LogError("Transform is not initialized!");
        yield break; // Early exit from the coroutine if the transform is null
    }
        var startPosition = _planetListContainerTransform.position;
        var endPosition = startPosition + target;

        var currentTime = 0.0f;

        while (currentTime < 1)
        {
            currentTime += Time.deltaTime * moveSpeed;
            _planetListContainerTransform.position = Vector3.Lerp(startPosition, endPosition, currentTime);
            yield return null;
        }

        // Ensure the final position is set
        _planetListContainerTransform.position = endPosition;
    }
}
