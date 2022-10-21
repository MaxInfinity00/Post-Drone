using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField]
    private float _moveToPlayerSpeed = 0.1f;
    private Rigidbody _rigidBody = null;
    private Transform _transform = null;
    private Transform _carryPosition = null;
    private bool _onSpot = false;
    private bool _pickedUp = false;

    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody>();
        _transform = this.transform;
        _carryPosition = transform;
    }

    public void Pickup()
    {
        var player = FindObjectOfType<PlayerPackageHandler>();
        _transform.parent = player.transform;
        _rigidBody.useGravity = false;
        _carryPosition = player.GetComponent<PlayerPackageHandler>().packageHoldPosition;
        _onSpot = false;
        _pickedUp = true;
    }
    public void Drop()
    {
        _transform.parent = null;
        _rigidBody.useGravity = true;
        _pickedUp = false;
    }

    private void MoveItemToCarrySpot()
    {

        if (_onSpot || !_pickedUp)
        {
            return;
        }
        else if (!_onSpot)
        {
            _transform.position = Vector3.Lerp(_transform.position, _carryPosition.position, _moveToPlayerSpeed);
            if ((_carryPosition.position - transform.position).sqrMagnitude <= 0.01f)
            {
                _transform.position = _carryPosition.position;
                _onSpot = true;
            }
        }

    }
    private void FixedUpdate()
    {
        MoveItemToCarrySpot();
    }
}
