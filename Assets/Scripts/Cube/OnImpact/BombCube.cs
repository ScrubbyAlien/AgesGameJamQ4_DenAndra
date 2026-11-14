using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BombCube : Cube
{
    [SerializeField] GameObject explosionPrefab;
    private bool blownUp = false;

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (state == BlockState.Inactive)
        {
            Connect(collider);
            return;
        }

        if (!collider.TryGetComponent(out Cube otherCube)) return;

        if (otherCube.state == BlockState.Inactive) return;

        if (isControlledByPlayer != otherCube.isControlledByPlayer)
        {
            BlowUp();
        }
    }

    private void BlowUp()
    {
        if (!blownUp)
        {
            blownUp = true;
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            OnDie();
        }
    }
}
