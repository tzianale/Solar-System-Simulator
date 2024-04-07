using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    [SerializeField] 
    private Camera gameCamera;
    
    private bool _targetSet;
    private Transform _target;
    private Vector3 _offset;

    // Update is called once per frame
    private void Update()
    {
        if (_targetSet)
        {
            gameCamera.transform.position = _target.position + _offset;
        }
    }

    public void SetTarget(Transform target, Vector3 offset)
    {
        _target = target;
        _offset = offset;
        
        _targetSet = true;
    }

    public void ClearTarget()
    {
        _targetSet = false;
    }
}
