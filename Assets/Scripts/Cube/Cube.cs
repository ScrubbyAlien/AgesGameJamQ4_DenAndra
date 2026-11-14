using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class Cube : MonoBehaviour
{
    public BlockState state;
    public int hp;
    public int collsionDmg;
    public bool isControlledByPlayer;

    [HideInInspector]
    public CubeConstruction controllingConstruction;

    public void OnCollisionEnter2D(Collision2D collider)
    {
        if (state == BlockState.Inactive)
        {
            Connect(collider);
        }

        if (state == BlockState.Controlled)
        {
            Cube otherCube = collider.gameObject.GetComponent<Cube>();
            if (otherCube != null)
            {
                otherCube.TakeDmg(collsionDmg);
            }
        }
    }

    public virtual void OnDie()
    {
        if (isControlledByPlayer)
        {
            //deatched other and die

            Destroy(gameObject);
        }
        else
        {
            //deatch and become inactive
            Debug.Log("Cube Died");
            state = BlockState.Inactive;
        }
    }
    public virtual void TakeDmg(int dmg)
    {
        if (state == BlockState.Inactive)
        {
            return;
        }

        Debug.Log(this + "took dmg" + dmg);

        hp = hp - dmg;

        if (hp <= 0)
        {
            hp = 0;

            OnDie();
        }
    }
    //Handle Connection
    public virtual void Connect(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent(out Cube otherCube)) return;

        if (otherCube.isControlledByPlayer)
        {
            // attempt to connect cube to player
            float playerRotation = otherCube.controllingConstruction.transform.eulerAngles.z;
            Debug.Log(playerRotation);
            Vector2Int direction = GetDirection(otherCube.transform.position, transform.position, playerRotation);
            if (otherCube.controllingConstruction.AddCube(this, otherCube, direction))
            {
                state = BlockState.Controlled;
                isControlledByPlayer = true;
            }
        }
    }

    private Vector2Int GetDirection(Vector3 attachToPosition, Vector3 attacheePosition, float rotation = 0f)
    {
        Vector3 direction = (attacheePosition - attachToPosition).normalized;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        { // right or left
            if (direction.x > 0) return CompensateRotation(rotation, Vector2Int.right);
            else return CompensateRotation(rotation, Vector2Int.left);
        }
        else
        { // up or down
            if (direction.y > 0) return CompensateRotation(rotation, Vector2Int.up);
            else return CompensateRotation(rotation, Vector2Int.down);
        }
    }

    private Vector2Int CompensateRotation(float rotation, Vector2Int direction)
    {
        rotation = Mathf.Round(rotation / 90) * 90; // round to nearest 90 deg
        rotation %= 360; // ensure rotation is positive
        while (rotation > 0)
        {
            (int x, int y) tuple = (direction.x, direction.y);
            tuple = tuple switch {
                (0, 1) => (1, 0),
                (1, 0) => (0, -1),
                (0, -1) => (-1, 0),
                (-1, 0) => (0, 1),
                _ => (0, 0)
            };
            Debug.Assert(tuple != (0, 0));
            direction = new Vector2Int(tuple.x, tuple.y);
            rotation -= 90;
        }
        return direction;
    }
}

public enum BlockState
{
    Inactive,
    Controlled
}