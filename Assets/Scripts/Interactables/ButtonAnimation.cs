using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject _buttonTop = null;
    [SerializeField]
    private string _soundName;

    private AudioManager audioManager = null;
    private Transform _buttonTransform = null;
    private Vector3 buttonMove = new Vector3(0f, -5f, 0f);
    bool _buttonPressed = false;

    private void Awake()
    {
        _buttonTransform = _buttonTop.transform;
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_buttonPressed)
        {
            ButtonPress();
        }
    }

    private void ButtonPress()
    {
        _buttonTransform.position = Vector3.Lerp(_buttonTransform.position, _buttonTransform.position + buttonMove, 0.1f);
        _buttonPressed = true;
        if (!string.IsNullOrEmpty(_soundName) && _buttonPressed)
        {
            audioManager.PlaySound(_soundName);
        }
        Invoke("ResetButton", 0.5f);
    }
    private void ResetButton()
    {
        _buttonPressed = false;
        _buttonTransform.position = Vector3.Lerp(_buttonTransform.position, _buttonTransform.position - buttonMove, 0.1f);
    }

}
