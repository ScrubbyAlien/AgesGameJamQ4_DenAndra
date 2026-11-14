using Microsoft.Win32.SafeHandles;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public DmgType dmgType;
    [SerializeField] public int dmg = 2;
    [SerializeField] public float speed = 3f;
    [SerializeField] public Rigidbody2D rb;
    private Cube shooter = null;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Cube cube = collision.GetComponent<Cube>();

        if(cube != null)
        {
            if(shooter == cube)
            {
                return;
            }
            cube.TakeDmg(dmg, dmgType);
            Destroy(gameObject);
        }

        

    }

    public virtual void SetUpProjectile(Vector3 direction, DmgType type, Cube cubeShooter)
    {
        shooter = cubeShooter;
    }
}
