using UnityEngine;

public class CannonProjectile : Projectile
{

    public override void SetUpProjectile(Vector3 direction, DmgType type)
    {
        rb.linearVelocity = (Vector2)direction.normalized * speed;
    }

    
}
