using UnityEngine;

public abstract class EnemyController : Controller
{
    [SerializeField]
    private PlayerStatePortal playerStatePortal;

    void FixedUpdate()
    {
        thrustThisFrame = GetThrustDirection(playerStatePortal);
        Debug.Log(thrustThisFrame);
        (turnThisFrame, turnDirection) = GetTurnDirection(playerStatePortal);

        UpdateRigidbody();

    }

    protected abstract Vector2 GetThrustDirection(PlayerStatePortal playerStatePortal);
    protected abstract (bool, float) GetTurnDirection(PlayerStatePortal playerStatePortal);

    protected Vector2 ToPlayer(PlayerStatePortal playerStatePortal)
    {
        return playerStatePortal.playerPosition - (Vector2)transform.position;
    }
}