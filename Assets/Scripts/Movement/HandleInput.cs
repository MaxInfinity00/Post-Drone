using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(DroneMovement),typeof(InteractInputHandler))]
public class HandleInput : MonoBehaviour
{
    private bool isPaused;
    private Controls _controls;
    private DroneMovement _drone;
    private InteractInputHandler _interactHandler;

    private void Awake()
    {
        isPaused = false;
        _controls = new Controls();
        _controls.Player.Enable();
        // _controls.Player.Dash.performed += OnDash;
        _controls.Player.Forward.performed += OnForward;
        _controls.Player.Interact.performed += OnInteract;
        _controls.Player.Pause.performed += Pause;

        _drone = GetComponent<DroneMovement>();
        _interactHandler = GetComponent<InteractInputHandler>();
    }

    private void OnDisable()
    {
        _controls.Player.Forward.performed -= OnForward;
        _controls.Player.Interact.performed -= OnInteract;
        _controls.Player.Pause.performed -= Pause;
    }

    // public void OnDash(InputAction.CallbackContext _) {
    //     _drone.Dash();
    // }

    public void OnForward(InputAction.CallbackContext context) 
    {
        if (!UIManager.instance.IsGamePaused())
            _drone.Move(context.ReadValue<float>());
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!UIManager.instance.IsGamePaused())
            _interactHandler.OnInteractPressed();
    }
    public void Pause(InputAction.CallbackContext context)
    {
        UIManager.instance.TogglePauseMenu();
    }

    private void FixedUpdate()
    {
        _drone.Elevation(_controls.Player.Elevate.ReadValue<float>());
        _drone.Turn(_controls.Player.Turn.ReadValue<float>());
        // _drone.Move(_controls.Player.Forward.ReadValue<float>());

    }
}