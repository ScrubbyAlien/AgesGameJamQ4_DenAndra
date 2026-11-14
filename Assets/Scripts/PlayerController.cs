using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    public event Action<Vector2> OnThrustInput;
    public event Action<float> OnTurnInput;

    private InputAction thrust;
    private InputAction turn;

    [SerializeField]
    private PlayerStatePortal playerStatePortal;

    private void Start()
    {
        thrust = InputSystem.actions.FindAction("Thrust");
        turn = InputSystem.actions.FindAction("Turn");
    }

    private void FixedUpdate()
    {
        playerStatePortal.playerPosition = transform.position;

        thrustThisFrame = thrust.ReadValue<Vector2>();
        OnThrustInput?.Invoke(thrustThisFrame);
        turnThisFrame = turn.IsPressed() && turning == null;

        if (turnThisFrame)
        {
            turnDirection = turn.ReadValue<float>();
            OnTurnInput?.Invoke(turnDirection);
        }

        UpdateRigidbody();
    }

    // BOOOOOOOM
}