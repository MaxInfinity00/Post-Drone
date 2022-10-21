using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] _pickupLocations;
    [SerializeField]
    private GameObject _pickupPrefab;

    private GameObject _currentActivePickup;
    private int _lastPickupLocation;
    public static PickupManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        foreach (Transform locations in _pickupLocations)
        {
            Instantiate(_pickupPrefab, _pickupLocations[_lastPickupLocation].position, Quaternion.identity);
        }
        // _lastPickupLocation = 0;
        // _currentActivePickup = Instantiate(_pickupPrefab, _pickupLocations[_lastPickupLocation].position, Quaternion.identity);
    }

    public void SpawnNewPickup()
    {
        int newLocation = Random.Range(0, _pickupLocations.Length + 1);

        if (newLocation == _lastPickupLocation)
        {
            SpawnNewPickup();
        }
        else
        {
            _currentActivePickup = Instantiate(_pickupPrefab, _pickupLocations[newLocation].position, Quaternion.identity);
            _lastPickupLocation = newLocation;
        }
    }
}
