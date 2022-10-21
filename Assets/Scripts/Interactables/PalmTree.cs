using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmTree : MonoBehaviour
{
    [SerializeField]
    private Interactable[] coconuts;

    private int _droppedCounter = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (_droppedCounter >= coconuts.Length)
        {
            return;
        }

        if (other.GetComponent<InteractInputHandler>() && _droppedCounter < coconuts.Length)
        {
            coconuts[_droppedCounter].Drop();
            _droppedCounter++;
        }

    }
}
