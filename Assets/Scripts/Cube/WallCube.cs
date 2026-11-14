using UnityEngine;

public class WallCube : Cube
{
    public bool killOnK;

    public override void OnDie()
    {
        base.OnDie();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && killOnK)
        {
            OnDie();
        }
    }
}