using Unity.VisualScripting;
using UnityEngine;

public class CanonCube : Cube
{
    [Header("Canon Cube")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private int cannonDmg;
    [SerializeField] private float reloadTime;
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
        //Debug.Log("Fire");
        GameObject Projectile = Instantiate(projectile);
    }
}
