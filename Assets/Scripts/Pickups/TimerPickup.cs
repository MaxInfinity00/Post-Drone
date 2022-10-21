using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Animator))]
public class TimerPickup : PickupScript
{

    [SerializeField]
    private float _addedTimeInSeconds = 20f;

    [SerializeField]
    private float _respawnTimer = 10f;

    [SerializeField]
    private Collider _objectCollider;
    [SerializeField]
    private ParticleSystem _pickupVFX;
    [SerializeField]
    private float _particleTime = 1.0f;

    [SerializeField]
    private GameObject _model;
    private Animator _animator;

    private void Awake()
    {
        _objectCollider.isTrigger = true;
        _animator = this.GetComponent<Animator>();
        var emission = _pickupVFX.emission;
        emission.enabled = false;
    }

    private void AddTime()
    {
        GameManager.instance.AddTime(_addedTimeInSeconds);
        PlaySound();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InteractInputHandler>())
        {
            AddTime();
            SpawnParticles();
            _animator.SetTrigger("PickedUp");
        }
    }

    private void RespawnPickup()
    {
        SetmodelsActivation(true);
        _objectCollider.enabled = true;
    }
    public void DisableObject()
    {
        Debug.Log($"called");
        SetmodelsActivation(false);
        _objectCollider.enabled = false;
        // StartCoroutine(RespawnTimer());
    }

    private IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(_respawnTimer);
        RespawnPickup();
    }

    //will find a better solution for deactivating models
    private void SetmodelsActivation(bool activation)
    {
        _model.SetActive(activation);
        Destroy(gameObject);
    }

    private void SpawnParticles()
    {
        if (_pickupVFX == null)
        {
            return;
        }
        var emission = _pickupVFX.emission;
        emission.enabled = true;
        _pickupVFX.Play();
        StartCoroutine(ResetSpawnParticles());
    }

    IEnumerator ResetSpawnParticles()
    {
        yield return new WaitForSeconds(_particleTime);
        var emission = _pickupVFX.emission;
        emission.enabled = false;
    }
}
