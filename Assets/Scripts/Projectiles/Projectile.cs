using Microsoft.Win32.SafeHandles;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour
{
    public DmgType dmgType;
    [SerializeField]
    public int dmg = 2;
    [SerializeField]
    public float speed = 3f;
    [SerializeField]
    public Rigidbody2D rb;
    private Cube shooter = null;

    private void Start()
    {
        Destroy(gameObject, 20f);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D is TilemapCollider2D)
        {
            Destroy(gameObject);
            return;
        }

        Cube cube = collider2D.GetComponent<Cube>();

        if (cube != null)
        {
            // enemies can't shoot walls in the level
            if (cube is WallCube && !cube.controllingConstruction && dmgType == DmgType.Enemy) return;

            if (shooter == cube)
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
        dmgType = type;
    }
}