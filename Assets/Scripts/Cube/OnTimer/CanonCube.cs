using Unity.VisualScripting;
using UnityEngine;

public class CanonCube : Cube
{
    [Header("Canon Cube")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float reloadTime;
    [SerializeField] private Transform shootPostion;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > reloadTime)
        {
            Fire();
            timer = 0;
        }
    }

    public void Fire()
    {
        if (state == BlockState.Inactive) return;
        //Debug.Log("Fire");
        GameObject projectileOb = Instantiate(projectilePrefab, shootPostion.position, shootPostion.rotation, null);
        Projectile projectile = projectileOb.GetComponent<Projectile>();
        if(projectile != null)
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
