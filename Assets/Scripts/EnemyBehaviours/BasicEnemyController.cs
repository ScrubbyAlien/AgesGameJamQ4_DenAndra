using UnityEngine;

public class BasicEnemyController : EnemyController
{
    [SerializeField]
    private float targetDistance;

    protected override Vector2 GetThrustDirection(PlayerStatePortal playerStatePortal)
    {
        Vector2 toPlayer = ToPlayer(playerStatePortal);
        if (toPlayer.magnitude > targetDistance) return toPlayer.normalized;
        else return Vector2.zero;
    }

    protected override (bool, float) GetTurnDirection(PlayerStatePortal playerStatePortal)
    {
        return (false, 0);
    }
}