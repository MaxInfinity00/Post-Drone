using System;
using UnityEngine;

[RequireComponent(typeof(DroneMovement))]
public class EngineAnimations : MonoBehaviour {
    [SerializeField] private Transform _engineLeft;
    [SerializeField] private Transform _engineRight;
    [Range(0f, 1f)] [SerializeField] private float _rotationLerp = 0.15f;

    [SerializeField] private float _pitchFactor = 60f;
    [SerializeField] private float _yawFactor = 60f;
    [SerializeField] private float _elevateFactor = 0.01f;

    private Vector3 _inputDirection = Vector3.zero;
    private Quaternion _targetRotationLeft;
    private Quaternion _targetRotationRight;
    private Vector3 _originalPositionLeft;
    private Vector3 _originalPositionRight;
    private Vector3 _targetPositionLeft;
    private Vector3 _targetPositionRight;

    private DroneMovement _droneMovement;

    private void Awake() {
        _droneMovement = GetComponent<DroneMovement>();
        _originalPositionLeft = _engineLeft.localPosition;
        _originalPositionRight = _engineRight.localPosition;
    }

    private void HandleRotationLeft() {
        float pitch = 0f, yaw = 0f, roll = 0f;

        if (_inputDirection.y < -0.1f) { //descending
            if (Mathf.Abs(_inputDirection.z) < 0.1f) { //not moving forward/backward
                if (Mathf.Abs(_inputDirection.x) < 0.1f) {
                    pitch = 175;
                }
                else {
                    pitch = _inputDirection.x * _pitchFactor * 2f;
                }
            }
            else if (_inputDirection.z > 0) { // if moving forward
                pitch = _pitchFactor / Mathf.Clamp(_inputDirection.z, 1, Mathf.Infinity) * 2;
                if (_inputDirection.x > 0) { //if turning right
                    yaw = _inputDirection.x * _yawFactor;
                }
            }
            else { // if moving backward
                pitch = -_pitchFactor * 2f;
                if (_inputDirection.x < 0) { //if turning left
                    yaw = _inputDirection.x * _yawFactor;
                }
            }
        }
        else if(_inputDirection.y > 0.1f){ // ascending
            if (Mathf.Abs(_inputDirection.z) < 0.1f) { //not moving forward/backward
                pitch = _inputDirection.x * _pitchFactor * 0.6f;
            }
            else if (_inputDirection.z > 0) { // if moving forward
                pitch = _pitchFactor / Mathf.Clamp(_inputDirection.z, 1, Mathf.Infinity) * 0.7f;
                if (_inputDirection.x > 0) { //if turning right
                    yaw = _inputDirection.x * _yawFactor;
                }
            }
            else { // if moving backward
                pitch = -_pitchFactor * 0.5f;
                if (_inputDirection.x < 0) { //if turning left
                    yaw = _inputDirection.x * _yawFactor;
                }
            }
        }
        else { // not descending
            if (Mathf.Abs(_inputDirection.z) < 0.1f) { //not moving forward/backward
                pitch = _inputDirection.x * _pitchFactor;
            }
            else if (_inputDirection.z > 0) { // if moving forward
                pitch = _pitchFactor / Mathf.Clamp(_inputDirection.z, 1, Mathf.Infinity);
                if (_inputDirection.x > 0) { //if turning right
                    yaw = _inputDirection.x * _yawFactor;
                }
            }
            else { // if moving backward
                pitch = -_pitchFactor;
                if (_inputDirection.x < 0) { //if turning left
                    yaw = _inputDirection.x * _yawFactor;
                }
            }
        }

        _targetRotationLeft = Quaternion.Euler(pitch,yaw, roll);
        _targetPositionLeft = _originalPositionLeft + _elevateFactor * _inputDirection.y * Vector3.up;
    }
    
    private void HandleRotationRight() {
        float pitch = 0, yaw = 0, roll = 0;
        if (_inputDirection.y < -0.1f) { //descending
            if (Mathf.Abs(_inputDirection.z) < 0.1f) { //not moving forward/backward
                if (Mathf.Abs(_inputDirection.x) < 0.1f) {
                    pitch = 175;
                }
                else {
                    pitch = _inputDirection.x * -_pitchFactor * 2f;
                }
            }
            else if (_inputDirection.z > 0) { // if moving forward
                pitch = _pitchFactor / Mathf.Clamp(_inputDirection.z, 1, Mathf.Infinity) * 2;
                if (_inputDirection.x > 0) { //if turning right
                    yaw = _inputDirection.x * _yawFactor;
                }
            }
            else { // if moving backward
                pitch = -_pitchFactor * 2;
                if (_inputDirection.x > 0) { //if turning right
                    yaw = _inputDirection.x * _yawFactor;
                }
            }
        }
        else if(_inputDirection.y > 0.1f){ // ascending
            if (Mathf.Abs(_inputDirection.z) < 0.1f) { //not moving forward/backward
                pitch = _inputDirection.x * -_pitchFactor * 0.6f;
            }
            else if (_inputDirection.z > 0) { // if moving forward
                pitch = _pitchFactor / Mathf.Clamp(_inputDirection.z, 1, Mathf.Infinity) * 0.7f;
                if (_inputDirection.x < 0) { //if turning left
                    yaw = _inputDirection.x * _yawFactor;
                }
            }
            else { // if moving backward
                pitch = -_pitchFactor * 0.5f;
                if (_inputDirection.x > 0) { //if turning right
                    yaw = _inputDirection.x * _yawFactor;
                }
            }
        }
        else { // not descending
            if (Mathf.Abs(_inputDirection.z) < 0.1f) { //not moving forward/backward

                pitch = _inputDirection.x * -_pitchFactor;
            }
            else if (_inputDirection.z > 0) { // if moving forward
                pitch = _pitchFactor / Mathf.Clamp(_inputDirection.z, 1, Mathf.Infinity);
                if (_inputDirection.x < 0) { //if turning left
                    yaw = _inputDirection.x * _yawFactor;
                }
            }
            else { // if moving backward
                pitch = -_pitchFactor;
                if (_inputDirection.x > 0) { //if turning right
                    yaw = _inputDirection.x * _yawFactor;
                }
            }
        }

        _targetRotationRight = Quaternion.Euler(pitch, yaw, roll);
        _targetPositionRight = _originalPositionRight + _elevateFactor * _inputDirection.y * Vector3.up;
    }
    private void FixedUpdate() {
        _inputDirection = _droneMovement.inputDirection;
        HandleRotationLeft();
        HandleRotationRight();
        _engineLeft.localRotation = Quaternion.Lerp(_engineLeft.localRotation, _targetRotationLeft,_rotationLerp);
        _engineRight.localRotation = Quaternion.Lerp(_engineRight.localRotation, _targetRotationRight,_rotationLerp);
        _engineLeft.localPosition = Vector3.Lerp(_engineLeft.localPosition, _targetPositionLeft, _rotationLerp);
        _engineRight.localPosition = Vector3.Lerp(_engineRight.localPosition, _targetPositionRight, _rotationLerp);
    }
}