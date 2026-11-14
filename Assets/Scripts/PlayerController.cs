using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public event Action<Vector2> OnThrustInput;
    public event Action<float> OnTurnInput;

    private Rigidbody2D body;
    private InputAction thrust;
    private InputAction turn;

    [SerializeField]
    private float thrustStrength, maxSpeed, rotationTime;

    private Coroutine turning;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        thrust = InputSystem.actions.FindAction("Thrust");
        turn = InputSystem.actions.FindAction("Turn");
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
        body.centerOfMass = Vector2.zero;
        Vector2 thrustThisFrame = thrust.ReadValue<Vector2>() * thrustStrength;
        bool turnThisFrame = turn.WasPerformedThisFrame() && turning == null;

        OnThrustInput?.Invoke(thrust.ReadValue<Vector2>() * thrustStrength);
        body.AddForce(thrustThisFrame);
        body.linearVelocity = Vector2.ClampMagnitude(body.linearVelocity, maxSpeed);

        if (turnThisFrame)
        {
            float turnDirection = turn.ReadValue<float>();
            OnTurnInput?.Invoke(turnDirection);
            float rotation = 90 * turnDirection;
            turning = StartCoroutine(Rotate(rotation));
        }
    }

    // BOOOOOOOM

    private IEnumerator Rotate(float rotation)
    {
        body.constraints = RigidbodyConstraints2D.None;
        body.angularVelocity = rotation / rotationTime;
        yield return new WaitForSeconds(rotationTime);
        body.angularVelocity = 0;

        float roundedToNearest90 = Mathf.Round(transform.eulerAngles.z / 90) * 90f;
        body.SetRotation(roundedToNearest90);
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        turning = null;
    }
}