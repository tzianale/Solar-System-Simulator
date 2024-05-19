using TMPro;
using UnityEngine;
using System.Collections;

public class PlanetListContainerScript : MonoBehaviour
{
    private Transform _planetListContainerTransform;
    private bool _listIsOpen;
    
    [SerializeField]
    private Vector3 moveDistance;
    
    [SerializeField]
    private float moveSpeed = 1.0f;
    
    public TextMeshProUGUI buttonText;

    
    private void Start()
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

    public void OnArrowClick()
    {
        Debug.Log("Arrow clicked");
        if (_listIsOpen)
        {
            StartCoroutine(MovePanel(moveDistance));
            _listIsOpen = false;
            buttonText.text = "↑";
        } else {
            StartCoroutine(MovePanel(-moveDistance));
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

        _planetListContainerTransform.position = endPosition;
    }
}
