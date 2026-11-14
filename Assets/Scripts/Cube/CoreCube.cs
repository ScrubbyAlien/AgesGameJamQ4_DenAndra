using UnityEngine;

public class CoreCube : Cube
{
    /// <inheritdoc />
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        return;
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