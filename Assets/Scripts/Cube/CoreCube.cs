using System;
using UnityEngine;

public class CoreCube : Cube
{
    public static event Action EnemyDied;
    public static event Action<int> PlayerTakeDamage;
    public override string prefabName => "";

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
    public override void TakeDmg(int dmg, DmgType type)
    {
        base.TakeDmg(dmg, type);
        if (isControlledByPlayer)
        {
            PlayerTakeDamage?.Invoke(currentHp);
        }
    }

    /// <inheritdoc />
    public override void EnterInactiveState()
    {
        DIE();
    }

    /// <inheritdoc />
    public override void OnDie()
    {
        currentHp = 0;
        if (isControlledByPlayer)
        {
            PlayerTakeDamage?.Invoke(currentHp);
            Debug.Log("lose game");
        }
        else
        {
            EnemyDied?.Invoke();
        }
        Disconnect();
    }

    private void DIE()
    {
        Destroy(gameObject);
        particles?.SetActive(true);
    }
}