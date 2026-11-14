using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Controller : MonoBehaviour
{
    private Rigidbody2D body;

    [SerializeField]
    private float thrustStrength, maxSpeed, dampening, rotationTime;

    protected Vector2 thrustThisFrame;

    protected Coroutine turning;
    protected bool turnThisFrame;
    protected float turnDirection;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    protected void UpdateRigidbody()
    {
        body.centerOfMass = Vector2.zero;

        body.AddForce(thrustThisFrame * thrustStrength);
        body.linearVelocity = Vector2.ClampMagnitude(body.linearVelocity, maxSpeed);
        if (thrustThisFrame.sqrMagnitude == 0)
        {
            body.linearVelocity *= dampening;
        }
        Debug.Log($"{gameObject.name} {body.linearVelocity}");

        if (turnThisFrame)
        {
            float rotation = 90 * turnDirection;
            turning = StartCoroutine(Rotate(rotation));
        }
    }

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