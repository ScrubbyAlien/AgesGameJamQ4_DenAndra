using UnityEngine;

public class SuggpropLogic : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    public void FireSuggprop()
    {
        rb.AddForce(new Vector2(0f, -200));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Cube otherCube)) return;

        if (otherCube.isControlledByPlayer)
        {
            //Run logic to steal;
            Steal(otherCube);
        }
    }

    public void Steal(Cube cube)
    {
        rb.linearVelocity = Vector3.zero;
        string CubePrefab = cube.prefabName;
        cube.TakeDmg(1000, DmgType.Enemy);



    }


}
