using System.Collections.Generic;
using UnityEngine;

public abstract class Cube : MonoBehaviour
{
    public BlockState state;
    public int baseHP;
    public int currentHp;
    public int collsionDmg;
    public bool isControlledByPlayer;
    [Min(0f)]
    public float pickUpRadius;

    private CircleCollider2D circleCollider;

    [SerializeField]
    private GameObject particles;

    [HideInInspector]
    public CubeConstruction controllingConstruction;
    [HideInInspector]
    public Vector2Int positionInConstruction;

    private void Awake()
    {
        circleCollider = gameObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = pickUpRadius;
        circleCollider.isTrigger = true;
    }

    private void Start()
    {
        currentHp = baseHP;
        particles.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (state == BlockState.Inactive)
        {
            Connect(collider);
        }

        if (state == BlockState.Controlled)
        {
            if (!collider.TryGetComponent(out Cube otherCube)) return;

            if (otherCube.state == BlockState.Inactive)
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

    protected virtual void OnTriggerStay2D(Collider2D collider)
    {
        if (state == BlockState.Inactive)
        {
            Connect(collider);
        }
    }

    public virtual void OnDie()
    {
        Disconnet();
        if (isControlledByPlayer)
        {
            //deatched other and die
            Destroy(gameObject);
        }
        else
        {
            //deatch and become inactive
            Debug.Log("Cube Died");
            EnterInactiveState();
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
    public virtual void Connect(Collider2D collider)
    {
        // todo: trigger connect animation

        if (!collider.TryGetComponent(out Cube otherCube)) return;

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
                particles.SetActive(false);
                //ConnectNeighbours(index);
            }
        }
    }

    public virtual void Disconnet()
    {
        controllingConstruction?.RemoveCube(this);
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

    public virtual void EnterInactiveState()
    {
        state = BlockState.Inactive;
        particles.SetActive(true);
    }

    private bool DirectionIsFree(Vector2Int direction)
    {
        if (!controllingConstruction) return true;
        // return true if there is no neighnbour in that direction
        return !controllingConstruction.GetCubeFromIndex(positionInConstruction + direction, out Cube _);
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