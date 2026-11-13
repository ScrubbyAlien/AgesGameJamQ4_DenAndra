using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public event Action<Vector2> OnThrustInput;
    public event Action<float> OnTurnInput;

    private Rigidbody body;
    private InputAction thrust;
    private InputAction turn;

    [SerializeField]
    private float thrustStrength, turnSpeed, maxSpeed, maxRotationSpeed;

    private void Start() {
        body = GetComponent<Rigidbody>();
        thrust = InputSystem.actions.FindAction("Thrust");
        turn = InputSystem.actions.FindAction("Turn");

        // add starting cubes
    }

    private void Update() {
        OnThrustInput?.Invoke(thrust.ReadValue<Vector2>() * thrustStrength);
        OnTurnInput?.Invoke(turn.ReadValue<float>() * turnSpeed);
    }
}