using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Cube : MonoBehaviour
{
    public BlockState state;
    public int baseHP;
    public int currentHp;
    public int collsionDmg;
    public bool isControlledByPlayer;

    private Dictionary<Vector2, bool> neighbours; 

    [HideInInspector]
    public CubeConstruction controllingConstruction;

    private void Start()
    {
        neighbours = new();
        currentHp = baseHP;
    }

    public void OnCollisionEnter2D(Collision2D collider)
    {
        if (state == BlockState.Inactive)
        {
            Connect(collider);
        }

        if (state == BlockState.Controlled)
        {
            if (!collider.collider.TryGetComponent(out Cube otherCube)) return;

            if(otherCube.state == BlockState.Inactive)
            {
                return;
            }

            if (isControlledByPlayer)
            {
                otherCube.TakeDmg(collsionDmg, DmgType.Player);
            }
            else
            {
                otherCube.TakeDmg(collsionDmg, DmgType.Enemy);
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
            currentHp = baseHP;
        }
    }
    public virtual void TakeDmg(int dmg, DmgType type)
    {
        if (state == BlockState.Inactive)
        {
            return;
        }
        //no dmg if player is 
        if (isControlledByPlayer && type == DmgType.Player) return;

        //no dmg if non player is vs enemy
        if (!isControlledByPlayer && type == DmgType.Enemy) return;

        currentHp = currentHp - dmg;

        if (currentHp <= 0)
        {
            currentHp = 0;

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
                controllingConstruction.GetIndexFromCube(this, out Vector2Int index);
                ConnectNeighbours(index);
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

    //Handles so the cube knows it neighbours and so on
    private void ConnectNeighbours(Vector2Int constructionPos)
    {
        Debug.Log(constructionPos);
        if(controllingConstruction.GetCubeFromIndex(constructionPos + Vector2Int.right, out Cube rightCube))
        {
            Debug.Log("I Found Neighbor" + Vector2.right, rightCube);
            neighbours.Add(Vector2.right, rightCube);
        }
        if (controllingConstruction.GetCubeFromIndex(constructionPos + Vector2Int.left, out Cube leftCube))
        {
            Debug.Log("I Found Neighbor" + Vector2.left, leftCube);
            neighbours.Add(Vector2.left, leftCube);
        }
        if (controllingConstruction.GetCubeFromIndex(constructionPos + Vector2Int.up, out Cube upCube))
        {
            Debug.Log("I Found Neighbor" + Vector2.up, upCube);
            neighbours.Add(Vector2.up, upCube);
        }
        if (controllingConstruction.GetCubeFromIndex(constructionPos + Vector2Int.down, out Cube downCube))
        {
            Debug.Log("I Found Neighbor" + Vector2.down, downCube);
            neighbours.Add(Vector2.down, downCube);
        }
    }
}


public enum BlockState
{
    Inactive,
    Controlled
}

public enum DmgType
{
    Player,
    Enemy,
    Neutral
}