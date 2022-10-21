using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    [SerializeField]
    private string _soundName;
    protected InteractInputHandler _player = null;
    protected AudioManager _audioManager = null;

    private void Awake()
    {
        _player = FindObjectOfType<InteractInputHandler>();
        _audioManager = FindObjectOfType<AudioManager>();
        if (_audioManager == null)
        {
            Debug.Log($"Audiomanager Empty");
        }
    }

    protected void PlaySound()
    {
        if (_audioManager == null)
        {
            _audioManager = FindObjectOfType<AudioManager>();
        }
        if (!string.IsNullOrEmpty(_soundName))
        {
            _audioManager.PlaySound(_soundName);
        }
    }
}
