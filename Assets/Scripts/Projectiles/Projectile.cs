using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public DmgType dmgType;
    [SerializeField] public int dmg = 2;
    [SerializeField] public float speed = 3f;
    [SerializeField] public Rigidbody2D rb;
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Cube cube = collision.collider.GetComponent<Cube>();

        if(cube != null)
        {
            cube.TakeDmg(dmg, dmgType);
        }

        Destroy(gameObject);

    }

    public virtual void SetUpProjectile(Vector3 direction, DmgType type)
    {

    }
}
