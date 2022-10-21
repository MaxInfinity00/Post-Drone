using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider),typeof(Rigidbody))]
public class Package : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField]
    private float _lerpSpeed = 0.1f;
    
    [SerializeField]
    private float _destructionTime = 5f;

    [SerializeField]
    private float _breakOutThreshold = 1000f;

    [SerializeField]
    private Collider _triggerArea;
    
    [SerializeField]
    private PackageType _packageType = PackageType.None;
    private PackageState _packageState = PackageState.Waiting;

    private Rigidbody _rigidBody;
    private Transform _transform;
    
    private bool _onSpot = true;
    private Transform _targetTransform;
    public DropoffPoint dropoffPoint;

    public PackageType GetPackageType() => _packageType;
    public void PickupPackage(Transform playerTransform, Transform holdPosition)
    {
        _transform.parent = playerTransform;
        _packageState = PackageState.PickedUp;
        _targetTransform = holdPosition;
        _onSpot = false;
        _rigidBody.useGravity = false;
        _rigidBody.isKinematic = true;
    }
    public void DeliverPackage(Transform dropoffPoint,Transform dropoffPosition) {
        _packageState = PackageState.Delivered;
        _transform.parent = dropoffPoint.transform;
        _onSpot = false;
        _targetTransform = dropoffPosition;
        StartCoroutine(DropoffPackage());
    }
    public void AccidentalDropPackage() {
        _packageState = PackageState.Dropped;
        _transform.parent = null;
        _rigidBody.useGravity = true;
        _rigidBody.isKinematic = false;
        _triggerArea.enabled = true;
        _transform.parent.GetComponent<PlayerPackageHandler>().DropPackage();
    }

    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody>();
        _transform = transform;
    }
    private void MovePackage() {
        if (_onSpot) return;
        _transform.position = Vector3.Lerp(_transform.position, _targetTransform.position, _lerpSpeed);
        if ((_targetTransform.position - transform.position).sqrMagnitude <= 0.01f) {
            _transform.position = _targetTransform.position;
            _onSpot = true;
        }
        // if (_packageState == PackageState.PickedUp)
        // {
        //     _movement = Vector3.Lerp(_transform.position, _transform.parent.position + _packagePositionToPlayer, _lerpSpeed);
        //     _transform.position = _movement;
        //     if (Vector3.Distance(_transform.position, _transform.parent.position) <= 0.1)
        //     {
        //         _onSpot = true;
        //     }
        // }
        // if (_packageState == PackageState.Delivered)
        // {
        //     _movement = Vector3.Lerp(_transform.position, _transform.parent.position, _lerpSpeed);
        //     _transform.position = _movement;
        // }
    }

    private IEnumerator DropoffPackage()
    {
        yield return new WaitForSeconds(_destructionTime);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        MovePackage();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.impulse.sqrMagnitude > _breakOutThreshold * _breakOutThreshold) {
            AccidentalDropPackage();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (_packageState != PackageState.Dropped) return;
        var player = other.GetComponent<PlayerPackageHandler>();
        if (!player || player.HasPackage()) return;
        
        // _packageManager.StartDrop();
        player.TakePackage(this);
        PickupPackage(player.transform,player.packageHoldPosition);
        _triggerArea.enabled = false;
    }
}
public enum PackageType
{
    None, BLUE, RED, YELLOW
};

public enum PackageState {
    Waiting, PickedUp, Dropped, Delivered
}

