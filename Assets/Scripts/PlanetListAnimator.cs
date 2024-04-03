using TMPro;
using UnityEngine;
using System.Collections;

public class PlanetListContainerScript : MonoBehaviour
{
    private Transform _planetListContainerTransform;
    private bool _listIsOpen;
    
    public float moveDistance = 210f; // The distance to move the panel.
    public float moveSpeed = 5.0f; // How fast the panel moves.
    public TextMeshProUGUI buttonText;

    // Start is called before the first frame update
    private void Start()
    {
        _planetListContainerTransform = transform;
    }

    public void OnArrowClick()
    {
        Debug.Log("Arrow clicked");
        if (_listIsOpen)
        {
            // Start moving down
            StartCoroutine(MovePanel(Vector3.down * moveDistance));
            _listIsOpen = false;
            buttonText.text = "↑";
        }
        else
        {
            // Start moving up
            StartCoroutine(MovePanel(Vector3.up * moveDistance));
            _listIsOpen = true;
            buttonText.text = "↓";
        }
    }

    private IEnumerator MovePanel(Vector3 target)
    {
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
