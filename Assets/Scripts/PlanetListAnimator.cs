using TMPro;
using UnityEngine;
using System.Collections;

public class PlanetListContainerScript : MonoBehaviour
{
    public Transform _planetListContainerTransform;
    public bool _listIsOpen;
    
    public float moveDistance = 210f; // The distance to move the panel.
    public float moveSpeed = 5.0f; // How fast the panel moves.
    public TextMeshProUGUI buttonText;

    // Start is called before the first frame update
    public void Start()
    {
        _planetListContainerTransform = transform;
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
