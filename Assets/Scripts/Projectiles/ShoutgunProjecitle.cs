using UnityEngine;

public class ShoutgunProjecitle : Projectile
{
    public override void SetUpProjectile(Vector3 direction, DmgType type, Cube shooter)
    {
        base.SetUpProjectile(direction, type, shooter);
        rb.linearVelocity = (Vector2)direction.normalized * speed;
    }
}
