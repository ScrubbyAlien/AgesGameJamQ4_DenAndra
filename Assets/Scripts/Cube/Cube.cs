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
        Debug.Log(state, this);
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
        Debug.Log(otherCube.name);

        if (otherCube.isControlledByPlayer)
        {
            //connect cube to player
            state = BlockState.Controlled;
            otherCube.controllingConstruction.AddCube(this, otherCube, Vector2Int.right);
        }
    }
    //
    // private Vector2Int GetDirection(Vector3 attachToPosition, Vector3 attacheePosition)
    // {
    //     Vector3 direction = (attacheePosition - attachToPosition).normalized;
    //
    // }
}

public enum BlockState
{
    Inactive,
    Controlled
}