using UnityEngine;

public class ShoutgunCube : Cube
{
    [Header("Canon Cube")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float reloadTime;
    [SerializeField] private Transform shootPostion;
    [SerializeField] private int nrOfBullets;
    [SerializeField] private int angleOfBullets;
    private float timer;

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
        //Debug.Log("Fire");
        float degreePerBullet = angleOfBullets / nrOfBullets;
        float startDegree = angleOfBullets * 0.5f;
        for(int i = 0; i < nrOfBullets; i++)
        {
            
            GameObject projectileOb = Instantiate(projectilePrefab, shootPostion.position, shootPostion.rotation, null);
            Projectile projectile = projectileOb.GetComponent<Projectile>();

            float currentOffset = startDegree - degreePerBullet * i;

            Quaternion spreedroation = Quaternion.Euler(0f, 0f, currentOffset);

            Vector2 spreedDirection = spreedroation * -transform.up;

            if (projectile != null)
            {
                if (isControlledByPlayer)
                {
                    projectile.SetUpProjectile(spreedDirection, DmgType.Player, this);
                }
                else
                {
                    projectile.SetUpProjectile(spreedDirection, DmgType.Enemy, this);
                }
            }

        }
    }
}
