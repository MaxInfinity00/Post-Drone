using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicatior : MonoBehaviour
{
    //Assign the target through editor for now.
    [SerializeField]
    private GameObject _target = null;
    [SerializeField]
    private float _hideDistance = 20f;

    private Transform _arrowPointer = null;
    private Vector3 _direction = Vector3.zero;
    private Transform _transform;
    private void Awake()
    {
        //Finds the canvas in the children to deactivate later
        _transform = transform;
        _arrowPointer = _transform.GetComponentInChildren<RectTransform>();
    }

    private void Update()
    {
        //Calculates the distance between the drone and the dropoff location
        _direction = _target.transform.position - _transform.position;
        //hides the arrow
        if (_direction.sqrMagnitude < _hideDistance * _hideDistance)
        {
            _arrowPointer.gameObject.SetActive(false);
        }
        else
        {
            _arrowPointer.gameObject.SetActive(true);
        }
        //calculates the angle to move it in the z plane
        var angleZ = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        _transform.rotation = Quaternion.AngleAxis(angleZ, Vector3.forward);
        //transform.LookAt(-_target.transform.position);
    }

}
