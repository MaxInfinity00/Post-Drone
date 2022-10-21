using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject door;

    [Header("Door positions")]
    [SerializeField] float doorXpos;
    [SerializeField] float doorYpos;
    [SerializeField] float doorZpos;


    private void OnTriggerEnter(Collider other)
    {
        door.transform.position = new Vector3(doorXpos, doorYpos, doorZpos);
    }
}
