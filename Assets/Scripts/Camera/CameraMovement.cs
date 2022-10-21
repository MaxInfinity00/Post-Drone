using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
public class CameraMovement : MonoBehaviour
{
    [Tooltip("Must be adjusted according to the camera's current")]
    [SerializeField]
    private Vector3 _desiredBoostCameraLocation = Vector3.zero;
    [Range(0, 1)]
    [SerializeField]
    private float _cameraMoveSpeed = 0.5f;
    [Range(30, 100)]
    [SerializeField]
    private float _boostLensFOV = 90;

    private Vector3 _startCameraLocation = Vector3.zero;
    private Vector3 _targetCameraLocation = Vector3.zero;
    private Cinemachine.CinemachineVirtualCamera _virtualCamera;
    private Cinemachine.CinemachineTransposer _virtualCameraTransposer;
    private float _startFOVValue = 0f;
    private float _desiredFOVValue = 0f;
    private bool _onPosition = true;
    void Start()
    {
        FindObjectOfType<DroneMovement>().BoostStatus += MoveBackCamera;
        _virtualCamera = this.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        _virtualCameraTransposer = _virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineTransposer>();
        _startCameraLocation = _virtualCameraTransposer.m_FollowOffset;
        _startFOVValue = _virtualCamera.m_Lens.FieldOfView;
    }

    private void MoveBackCamera(bool boosted)
    {
        if (boosted)
        {
            _onPosition = false;
            _targetCameraLocation = _desiredBoostCameraLocation;
            _desiredFOVValue = _boostLensFOV;
        }
        else if (!boosted)
        {
            if (_targetCameraLocation != _startCameraLocation)
            {
                _targetCameraLocation = _startCameraLocation;
                _desiredFOVValue = _startFOVValue;
            }
        }
    }

    private void MoveCamera()
    {
        Vector3 currentLocation = _virtualCameraTransposer.m_FollowOffset;
        currentLocation = Vector3.Lerp(currentLocation, _targetCameraLocation, _cameraMoveSpeed);
        _virtualCameraTransposer.m_FollowOffset = currentLocation;
        float currentFOVValue = _virtualCamera.m_Lens.FieldOfView;
        currentFOVValue = Mathf.Lerp(currentFOVValue, _desiredFOVValue, _cameraMoveSpeed);
        _virtualCamera.m_Lens.FieldOfView = currentFOVValue;

    }

    private void LateUpdate()
    {
        if (!_onPosition)
        {
            MoveCamera();
            if (_virtualCameraTransposer.m_FollowOffset == _desiredBoostCameraLocation)
            {
                _onPosition = true;
            }
        }
    }

}
