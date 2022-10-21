using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPackageHandler : MonoBehaviour {
    public Transform packageHoldPosition;
    [HideInInspector] public Package currentPackage;
    
    private List<Sound> _activeSounds = new List<Sound>();
    private AudioManager _audioManager;
    [Header("Sounds")]
    [SerializeField]
    private string _pickupSound;
    [SerializeField]
    private string _dropSound;

    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    public bool HasPackage() => currentPackage != null;

    public void TakePackage(Package package) {
        currentPackage = package;
        if (!string.IsNullOrEmpty(_pickupSound))
        {
            _audioManager.PlaySound(_pickupSound);
        }
    }

    public void DropPackage() {
        currentPackage = null;
        if (!string.IsNullOrEmpty(_dropSound))
        {
            _audioManager.PlaySound(_dropSound);
        }
    }
}
