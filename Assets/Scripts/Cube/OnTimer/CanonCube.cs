using Unity.VisualScripting;
using UnityEngine;

public class CanonCube : Cube
{
    public override string prefabName => "Canon";

    [Header("Canon Cube")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private float reloadTime;
    [SerializeField]
    private Transform shootPostion;
    private float timer;
    private Vector2Int pointingDirection = Vector2Int.down;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > reloadTime)
        {
            Fire();
            timer = 0;
        }
    }

    public void Fire()
    {
        if (state == BlockState.Inactive) return;
        // Vector2Int direction = CompensateRotation(-transform.eulerAngles.z, pointingDirection);
        // Debug.Log($"{-transform.eulerAngles.z} {direction}");
        // if (!controllingConstruction || !controllingConstruction.DirectionIsFree(positionInConstruction, direction))
        // {
        //     return;
        // }

        GameObject projectileOb = Instantiate(projectilePrefab, shootPostion.position, shootPostion.rotation, null);
        Projectile projectile = projectileOb.GetComponent<Projectile>();
        if (projectile != null)
        {
            if (isControlledByPlayer)
            {
                projectile.SetUpProjectile(-transform.up, DmgType.Player, this);
            }
            else
            {
                projectile.SetUpProjectile(-transform.up, DmgType.Enemy, this);
            }
        }
    }
}