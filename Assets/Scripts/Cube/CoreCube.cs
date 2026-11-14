using UnityEngine;

public class CoreCube : Cube
{
    /// <inheritdoc />
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (state == BlockState.Controlled)
        {
            if (!collider.TryGetComponent(out Cube otherCube)) return;

            if (otherCube.state == BlockState.Inactive)
            {
                return;
            }

            if (isControlledByPlayer)
            {
                otherCube.TakeDmg(collsionDmg, DmgType.Player);
            }
        }
        return;
    }

    /// <inheritdoc />
    protected override void OnTriggerStay2D(Collider2D collider)
    {
        if (isControlledByPlayer)
        {
            base.OnTriggerStay2D(collider);
        }
    }

    /// <inheritdoc />
    public override void OnDie()
    {
        Disconnect();
        Destroy(gameObject);

        particles?.SetActive(true);

        if (isControlledByPlayer)
        {
            // todo: lose game
        }
    }
}