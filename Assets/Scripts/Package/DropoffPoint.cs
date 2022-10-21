using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider))]
public class DropoffPoint : MonoBehaviour
{
    // [SerializeField] private PackageType _askedPackageType = PackageType.None;
    // [SerializeField] private GameObject Bubble;
    // [SerializeField] private MeshRenderer Indicator;
    [SerializeField] private Transform _dropoffPosition;
    [SerializeField]
    private float _timeToAddInSeconds = 60;

    [SerializeField] private GameObject _beacon;

    public bool GetBeaconActive() => _beacon.activeInHierarchy;

    public event Action<float> PackageDelivered = delegate { };
    // public void CreateRequest()
    // {
    //     int randomPackage = Random.Range(1, Enum.GetValues(typeof(PackageType)).Length);
    //     _askedPackageType = (PackageType)randomPackage;
    //     Bubble.SetActive(true);
    //     Indicator.material = PackageRequestManager.instance.PackageMaterials[randomPackage - 1];
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DroneMovement>())
        {
            CollectPickup(other.transform);
        }
    }

    protected virtual void CollectPickup(Transform target)
    {
        var player = target.GetComponent<PlayerPackageHandler>();
        if (!player || !player.currentPackage) return;
        // player.DropPackage();//Handles the dropping If we want to drop wrong packages
        if (this == player.currentPackage.dropoffPoint)
        {
            PackageDelivered(_timeToAddInSeconds);
            // _askedPackageType = PackageType.None;
            // Bubble.SetActive(false);

            player.currentPackage.DeliverPackage(transform, _dropoffPosition);
            player.DropPackage(); //Handles the dropping If we want to drop only the right packages

            PackageRequestManager.instance.PackageDropped(this);
            SetBeacon(false);
        }
    }

    public void SetBeacon(bool v)
    {
        _beacon.SetActive(v);
    }
}
