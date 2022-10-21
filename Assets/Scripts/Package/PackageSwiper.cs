using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageSwiper : DropoffPoint
{
    private Animator _swiperAnimator;

    private Transform _player;
    private Vector3 _swiperPosition;
    private Transform _swiperTransform;
    private void Awake()
    {
        _swiperAnimator = this.GetComponent<Animator>();
        _swiperTransform = this.transform;
        _swiperPosition = _swiperTransform.position;
        _player = FindObjectOfType<DroneMovement>().GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DroneMovement>())
        {
            CollectPickup(other.transform);
            _swiperAnimator.SetBool("PlayerInRange", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        _swiperAnimator.SetBool("PlayerInRange", false);
    }
    private void PlayAskAnimations()
    {
        if (GetBeaconActive())
        {
            _swiperAnimator.SetBool("PackagePickedUp", true);
            Vector3 lookAtPosition = new Vector3(_player.position.x, _swiperPosition.y, _player.position.z);
            _swiperTransform.LookAt(lookAtPosition);
        }
        else
        {
            _swiperAnimator.SetBool("PackagePickedUp", false);
        }
    }
    private void Update()
    {
        PlayAskAnimations();
    }

}
