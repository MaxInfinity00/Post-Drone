using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float throwForce;
    
    [SerializeField]
    private float _humLoopTime = 6.8f;

    private List<Sound> _activeSounds = new List<Sound>();
    private AudioManager _audioManager;
    [Header("Sounds")]
    [SerializeField]
    private string _badHum;
    [SerializeField]
    private string _badHit;

    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        _audioManager = FindObjectOfType<AudioManager>();
        Debug.Log($"{_audioManager}");
        if (!string.IsNullOrEmpty(_badHum))
        {
            StartCoroutine(HumLoop());
        }
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        DroneMovement droneMovement = other.GetComponent<DroneMovement>();

        if (droneMovement == null)
        {
            Debug.Log($"Drone Movement Not Found");
            return;
        }
        else
        {
            droneMovement.YeetThePlayer(transform, throwForce);
            animator.SetTrigger("Attack");
            if (!string.IsNullOrEmpty(_badHit))
            {
                _audioManager.PlaySound(_badHit);
            }
        }
    }
    IEnumerator HumLoop()
    {
        _audioManager.PlaySoundAtSpot(_badHum, transform.position, 1f);
        yield return new WaitForSeconds(_humLoopTime);
        StartCoroutine(HumLoop());
    }
}
