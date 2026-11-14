using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BombCube : Cube
{
    [SerializeField]
    GameObject explosionPrefab;
    private bool blownUp = false;

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (state == BlockState.Inactive)
        {
            blownUp = false;
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

    public override void TakeDmg(int dmg, DmgType type)
    {
        BlowUp();
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