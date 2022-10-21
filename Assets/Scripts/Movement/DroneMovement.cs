using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    [Header("Movement")]

    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private float _elevationSpeed = 2f;
    [SerializeField]
    private float _turnSpeed = 2f;
    [Range(0, 1)]
    [SerializeField]
    private float _rampUpSpeed = 0.1f;
    [Range(0, 1)]
    [SerializeField]
    private float _rampDownSpeed = 0.1f;
    [SerializeField]
    private float _bumpResetTime = 1f;
    [SerializeField]
    private float _bumpStrength = 3;

    private float _desiredZValue = 0;

    public float GetBumpStrength() => _bumpStrength;
    public float SetBumpStrength() => _bumpStrength;

    [Header("Rolling movement")]
    // [SerializeField]
    // private float _pitchFactorDueElevation = -3.0f;
    [SerializeField]
    private float _pitchFactorDueMovement = 30f;
    [SerializeField]
    private Vector2 _pitchClamp = new Vector2(-1, 2.5f);
    [SerializeField]
    private float _rollFactorDueMovement = -30f;

    [Header("Gravity")]
    [SerializeField] private bool gravity = false;
    [SerializeField] private float gravityForce = 2f;

    [Space]

    [SerializeField]
    [Range(0f, 1f)]
    private float _rotationDamping = 0.15f;


    public Vector3 inputDirection = Vector3.zero;
    [SerializeField]
    private GameObject _body = null;
    private float _flySpeed = 0;
    private float _currentSpeed = 0f;
    private Quaternion _targetRotation;
    private Transform _transform;
    private Rigidbody _rigidbody;

    [Header("Speed Mode")]
    [SerializeField] private Gear gear = Gear.Neutral;
    [SerializeField] private float[] speeds = new float[5] { -1, 0, 1, 1.5f, 2.5f };


    private List<Sound> _activeSounds = new List<Sound>();
    private AudioManager _audioManager;
    [Header("Sounds")]
    [SerializeField]
    private string _boostSound;
    private bool boost;
    [SerializeField]
    private string _hitSound;
    [SerializeField]
    private string _droningSound;
    [SerializeField]
    private string _movementSound;
    [Range(0, 3)]
    [SerializeField]
    private float _normalPitchSound = 1;
    [Range(0, 3)]
    [SerializeField]
    private float _fastPitchSound = 1;
    [Range(0, 3)]
    [SerializeField]
    private float _fastestPitchSound = 1;
    [Header("Particles")]
    [SerializeField]
    private ParticleSystem[] _boosters;
    [SerializeField]
    private float _boosterTime = 1f;

    private bool _isBumpedToSomething = false;


    //Events
    public event Action<bool> BoostStatus = delegate { };

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void InitializeMovement()
    {
        _currentSpeed = _moveSpeed * Time.fixedDeltaTime;
        _flySpeed = _elevationSpeed * Time.fixedDeltaTime;
        _audioManager = FindObjectOfType<AudioManager>();
        if (!string.IsNullOrEmpty(_droningSound))
        {
            _audioManager.PlaySound(_droningSound);
        }
    }

    //Moves according to the incomming input, to avoid tank/like movement minds the forward of the transform
    //While making the input just send the vector 2 result to here
    public void Move(float value)
    {
        if (!_isBumpedToSomething)
        {
            if (value == -1)
            {
                if ((int)gear == 0)
                {
                    return;
                }
                else
                {
                    gear--;
                }
            }
            else if (value == 1)
            {
                if ((int)gear == Enum.GetValues(typeof(Gear)).Length - 1)
                {
                    //gear = Gear.Neutral;
                    return;
                }
                else
                {
                    gear++;
                }
            }
        }
        // _transform.Translate(Vector3.forward * value * _currentSpeed * boostMultiplier);
        // _rigidbody.MovePosition(_transform.TransformPoint(Vector3.forward * value * _currentSpeed * boostMultiplier));
        //_inputDirection.z = speeds[(int)gear >= speeds.Length ? speeds.Length - 1 : (int)gear];//Temporary Solution to rolling movement, if not included rolling while elevating gets canceled when player starts to move
        _desiredZValue = speeds[(int)gear >= speeds.Length ? speeds.Length - 1 : (int)gear];
        // Debug.Log(speeds.Length);
    }

    public void Turn(float value)
    {
        if (!_isBumpedToSomething)
        {
            _transform.Rotate(Vector3.up, value * _turnSpeed);
            inputDirection.x = value;
        }
    }

    //I seperated these since I think we'll have different input keys for them
    public void Elevation(float value)
    {
        // _transform.Translate(Vector3.up * value * _flySpeed * boostMultiplier);
        if (!string.IsNullOrEmpty(_movementSound))
        {
            _audioManager.PlaySound(_movementSound);
        }

        inputDirection.y = Mathf.Lerp(inputDirection.y, value, _rampUpSpeed);
    }

    //I think this is what you said to me in discord about rotating with the movement
    //Quite buggy attm will figure out later
    private void HandleRotation()
    {
        // float pitchDuePostion = transform.localPosition.y * _pitchFactorDueElevation;

        float pitchDueForwardMove = Mathf.Clamp((_desiredZValue == 0 ? -inputDirection.y : (inputDirection.y == 1 ? -_desiredZValue : _desiredZValue)), _pitchClamp.x, _pitchClamp.y) * _pitchFactorDueMovement;

        // float pitch = pitchDuePostion + pitchDueForwardMove;
        float pitch = pitchDueForwardMove;
        float yaw = 0f;
        float roll = _desiredZValue == 0 ? 0 : inputDirection.x * _rollFactorDueMovement;
        // float roll = 0f;
        _targetRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void HandleMovementSound()
    {
        switch (gear)
        {
            case Gear.Vroom:
                if (!string.IsNullOrEmpty(_movementSound))
                {
                    _audioManager.ChangePitch(_movementSound, _normalPitchSound);
                    _audioManager.PlaySound(_movementSound);
                }
                break;
            case Gear.Nyoom:
                if (!string.IsNullOrEmpty(_movementSound))
                {
                    _audioManager.ChangePitch(_movementSound, Mathf.Lerp(_audioManager.GetPitch(_movementSound), _fastPitchSound, _rampUpSpeed));
                    _audioManager.PlaySound(_movementSound);
                }
                break;
            case Gear.SuperNyoom:
                if (!string.IsNullOrEmpty(_movementSound))
                {
                    _audioManager.ChangePitch(_movementSound, Mathf.Lerp(_audioManager.GetPitch(_movementSound), _fastestPitchSound, _rampUpSpeed));
                    _audioManager.PlaySound(_movementSound);
                }
                break;
            case Gear.Reverse:
                if (!string.IsNullOrEmpty(_movementSound))
                {
                    _audioManager.ChangePitch(_movementSound, _normalPitchSound);
                    _audioManager.PlaySound(_movementSound);
                }
                break;
            default:
                if (!string.IsNullOrEmpty(_movementSound))
                {
                    _audioManager.StopSound(_movementSound);
                }
                break;
        }

        if (gear == Gear.SuperNyoom && !string.IsNullOrEmpty(_boostSound) && !boost)
        {
            boost = true;
            //For changeing the camera with boosting 
            BoostStatus(boost);
            _audioManager.PlaySound(_boostSound);
        }
        else if (gear == Gear.Nyoom)
        {
            boost = false;
            //For changeing the camera with boosting 
            BoostStatus(boost);
        }
    }

    private void Start()
    {
        InitializeMovement();
    }

    private void Update()
    {
        HandleRotation();
        PlayBoosterAnims();
        _body.transform.localRotation = Quaternion.Lerp(_body.transform.localRotation, _targetRotation, _rotationDamping);
    }

    private void FixedUpdate()
    {
        if (!_isBumpedToSomething)
        {
            if (gravity) _transform.Translate(gravityForce * Time.fixedDeltaTime * Vector3.down, Space.World);
            if (gear == Gear.Neutral)
            {
                inputDirection.z = Mathf.Lerp(inputDirection.z, _desiredZValue, _rampDownSpeed);
            }
            else
            {
                inputDirection.z = Mathf.Lerp(inputDirection.z, _desiredZValue, _rampUpSpeed);
            }
            _rigidbody.velocity = GetVelocity();
        }
        HandleMovementSound();
    }
    private Vector3 GetVelocity()
    {
        // return new Vector3(0f,_inputDirection.y * _elevationSpeed, _inputDirection.z * _moveSpeed) * boostMultiplier;
        return (inputDirection.y * _elevationSpeed * Vector3.up) +
               (inputDirection.z * _moveSpeed * _transform.forward);
    }
    private void BumpedToObjects(Collision other)
    {
        if (!string.IsNullOrEmpty(_hitSound))
        {
            _audioManager.PlaySound(_hitSound);
        }
        _isBumpedToSomething = true;
        BoostStatus(false);
        gear = Gear.Neutral;
        Vector3 hitDireciton = (_transform.position - other.transform.position).normalized;
        hitDireciton *= _bumpStrength;
        _rigidbody.velocity = hitDireciton;
        _desiredZValue = 0;
        inputDirection.z = 0;
        StartCoroutine(ResetBump());
    }

    IEnumerator ResetBump()
    {
        yield return new WaitForSeconds(_bumpResetTime);
        _isBumpedToSomething = false;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (gear == Gear.Nyoom || gear == Gear.SuperNyoom)
        {
            BumpedToObjects(other);
        }
    }

    //Used for extreme bouncing of the player, changes the bouncing strength and reverts it back afterwards
    public void YeetThePlayer(Transform bouncer, float bouncePower)
    {
        _isBumpedToSomething = true;
        BoostStatus(false);
        gear = Gear.Neutral;
        Vector3 hitDireciton = (_transform.position - bouncer.transform.position).normalized;
        hitDireciton *= bouncePower;
        _rigidbody.velocity = hitDireciton;
        _desiredZValue = 0;
        inputDirection.z = 0;
        StartCoroutine(ResetBump());
    }

    private void PlayBoosterAnims()
    {
        if (gear == Gear.SuperNyoom && !boost)
        {
            foreach (ParticleSystem booster in _boosters)
            {
                var emission = booster.emission;
                emission.enabled = true;
                booster.Play();
            }
            StartCoroutine(ResetBoosterParticles());
        }
    }

    IEnumerator ResetBoosterParticles()
    {
        yield return new WaitForSeconds(_boosterTime);
        foreach (ParticleSystem booster in _boosters)
        {
            var emission = booster.emission;
            emission.enabled = false;

        }
    }

    public int WhatGearAmI()
    {


        return (int)gear;
    }
}

public enum Gear
{
    Reverse,
    Neutral,
    Vroom,
    Nyoom,
    SuperNyoom
}
