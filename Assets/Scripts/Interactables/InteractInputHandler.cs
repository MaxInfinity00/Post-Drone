using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractInputHandler : MonoBehaviour
{
    [SerializeField]
    private float _interactDistance = 20;
    private float closestInteractableDistance = Mathf.Infinity;
    private Interactable[] _interactables;
    private Interactable closestInteractable;
    private bool _pickedUp = false;


    private void Awake()
    {
        _interactables = FindObjectsOfType<Interactable>();
    }


    public void OnInteractPressed()
    {
        if (!_pickedUp)
        {
            FindClosestIntercatable();
            if (closestInteractableDistance < _interactDistance)
            {
                closestInteractable.Pickup();
                _pickedUp = true;
            }
        }
        else if (_pickedUp)
        {
            closestInteractable.Drop();
            _pickedUp = false;
            closestInteractable = null;
            closestInteractableDistance = Mathf.Infinity;
        }

    }
    private void FindClosestIntercatable()
    {
        if (_interactables == null) return;
        foreach (Interactable testedObject in _interactables)
        {
            if (closestInteractableDistance * closestInteractableDistance > (testedObject.transform.position - transform.position).sqrMagnitude)
            {
                closestInteractable = testedObject;
                closestInteractableDistance = Vector3.Distance(testedObject.transform.position, transform.position);
            }
        }
    }
}
