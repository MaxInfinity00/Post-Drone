using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberBounce : MonoBehaviour
{


    [SerializeField] private float rubberPower;
    private Animator animator;

    [SerializeField] private Material[] rubberColours;

    private MeshRenderer mesh;
    private int rubberInt;

    [SerializeField] private GameObject selfDestroyingGlitter;

    private void Start()
    {
        animator = GetComponent<Animator>();
        mesh = transform.GetChild(1).transform.GetComponent<MeshRenderer>();
        rubberInt = 0;
    }

    private void OnTriggerEnter(Collider other)
    {

        DroneMovement droneMovement = other.transform.GetComponent<DroneMovement>();
        if(droneMovement != null)
        {
            animator.SetTrigger("Bounce");
            droneMovement.YeetThePlayer(transform, rubberPower);
            //animator.SetTrigger("Bounce");
            ChangeColour();

        }
        


    }
    private void ChangeColour()
    {
        rubberInt += 1;
        rubberInt %= rubberColours.Length;
        mesh.material = rubberColours[rubberInt];
        Instantiate(selfDestroyingGlitter, transform.GetChild(2).position, transform.rotation);



       // mesh.material = currentPlayer %= currentAmountOfPlayers


    }
}
