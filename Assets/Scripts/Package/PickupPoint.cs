using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickupPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject _packagePrefab;

    [SerializeField]
    private Transform _spawnPoint = null;


    [SerializeField] private List<DropoffPoint> _droppoffPoints;
    [SerializeField] private GameObject _beacon;
    [SerializeField]
    private ParticleSystem _spawnVFX;
    [SerializeField]
    private float _particleTime = 1.0f;

    private int _packageIndex = 0;
    [HideInInspector] public Package package = null;

    //When collided object has droneMovement component stickts the package to it
    private void OnTriggerEnter(Collider other)
    {
        PickupPackage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        PickupPackage(other);
    }

    private void PickupPackage(Collider other)
    {
        if (!package) return;
        var player = other.GetComponent<PlayerPackageHandler>();
        if (!player || player.HasPackage()) return;

        player.TakePackage(package);
        package.PickupPackage(player.transform, player.packageHoldPosition);
        PackageRequestManager.instance.SetAllPickupBeacons(false);
        package.dropoffPoint.SetBeacon(true);
        package = null;
    }

    public void SpawnPackage()
    {
        package = Instantiate(_packagePrefab, _spawnPoint.position, Quaternion.identity, transform).GetComponent<Package>();
        package.dropoffPoint = _droppoffPoints[_packageIndex];
        _packageIndex++;
        SpawnParticles();
    }

    public void SetBeacon(bool v)
    {
        _beacon.SetActive(v);
    }

    private void SpawnParticles()
    {
        if (_spawnVFX == null)
        {
            return;
        }
        var emission = _spawnVFX.emission;
        emission.enabled = true;
        _spawnVFX.Play();
        StartCoroutine(ResetSpawnParticles());
    }

    IEnumerator ResetSpawnParticles()
    {
        yield return new WaitForSeconds(_particleTime);
        var emission = _spawnVFX.emission;
        emission.enabled = false;
    }
}



