using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PackageRequestManager : MonoBehaviour {
    // [SerializeField] private List<DropoffPoint> dropoffPoints;
    // [SerializeField] private DropoffPoint primaryDropOffPoint;
    // [SerializeField] private int noOfActiveDrops;
    // public List<Material> PackageMaterials;

    // private List<DropoffPoint> currentlyActive = new List<DropoffPoint>();

    [SerializeField] private List<PickupPoint> _pickupPoints;
    [SerializeField] private int _maximumSpawns = 9;
    private int _spawnIndex;

    public static PackageRequestManager instance;

    // public void UpdateDropoffPoints() {
    //     dropoffPoints = FindObjectsOfType<DropoffPoint>().ToList();
    // }

    public void Awake() {
        instance = this;
        // if (noOfActiveDrops > dropoffPoints.Count) noOfActiveDrops = dropoffPoints.Count;
        // if (dropoffPoints.Count == noOfActiveDrops) {
        //     foreach (DropoffPoint dropoffPoint in dropoffPoints) {
        //         dropoffPoint.CreateRequest();
        //         currentlyActive.Add(dropoffPoint);
        //     }
        //     return;
        // }
        //
        // primaryDropOffPoint.CreateRequest();
        // currentlyActive.Add(primaryDropOffPoint);
        //
        // int i = 0;
        // while (i < noOfActiveDrops - 1) {
        //     int randomPoint = Random.Range(0, dropoffPoints.Count - 1);
        //     if (!currentlyActive.Contains(dropoffPoints[randomPoint])) {
        //         dropoffPoints[randomPoint].CreateRequest();
        //         currentlyActive.Add(dropoffPoints[randomPoint]);
        //         i++;
        //     }
        // }
        SpawnPackage();
    }

    public void PackageDropped(DropoffPoint dropoffPoint) {
        UIManager.instance.SendWinScore();
        // if (dropoffPoints.Count == noOfActiveDrops) {
        //     dropoffPoint.CreateRequest();
        //     return;
        // }
        // while (true) {
        //     int randomPoint = Random.Range(0, dropoffPoints.Count - 1);
        //     if (!currentlyActive.Contains(dropoffPoints[randomPoint])) {
        //         currentlyActive.Remove(dropoffPoint);
        //         dropoffPoints[randomPoint].CreateRequest();
        //         currentlyActive.Add(dropoffPoints[randomPoint]);
        //         break;
        //     }
        // }
        SpawnPackage();
    }

    public void SpawnPackage() {
        if (_spawnIndex < _pickupPoints.Count) {
            _pickupPoints[_spawnIndex].SpawnPackage();
        }
        else if(_spawnIndex % _pickupPoints.Count == 0 && _spawnIndex < _maximumSpawns){
            foreach (PickupPoint pickupPoint in _pickupPoints) {
                pickupPoint.SpawnPackage();
            }
        }
        SetAllPickupBeacons(true);
        _spawnIndex++;
    }

    public void SetAllPickupBeacons(bool v) {
        if (v) {
            foreach (PickupPoint pickupPoint in _pickupPoints) {
                if(pickupPoint.package)
                    pickupPoint.SetBeacon(true);
            }
        }
        else {
            foreach (PickupPoint pickupPoint in _pickupPoints) {
                pickupPoint.SetBeacon(false);
            }
        }
    }
}