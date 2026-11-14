using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public abstract class Cube : MonoBehaviour
{
    public BlockState state;
    public int baseHP;
    [ReadOnly]
    public int currentHp;
    public int collsionDmg;
    public bool isControlledByPlayer;
    [Min(0f)]
    public float pickUpRadius;

    private CircleCollider2D circleCollider;

    [SerializeField]
    protected GameObject particles;

    [HideInInspector]
    public CubeConstruction controllingConstruction;
    [HideInInspector]
    public Vector2Int positionInConstruction;

    [Header("Animatior")]
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        circleCollider = gameObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = pickUpRadius;
        circleCollider.isTrigger = true;
    }

    private void Start()
    {
        currentHp = baseHP;
        particles.SetActive(state == BlockState.Inactive);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (state == BlockState.Inactive)
        {
            Connect(collider);
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == BlockState.Controlled)
        {
            if (!collision.collider.TryGetComponent(out Cube otherCube)) return;

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
        if (animator != null)
        {
            animator.SetTrigger("Red");
        }
        if (isControlledByPlayer)
        {
            //deatched other and die
            Disconnect();
            Destroy(gameObject);
        }
        else
        {
            //deatch and become inactive
            Disconnect();
            Debug.Log("Cube Died");
            EnterInactiveState();
            currentHp = baseHP;
        }
    }
    public virtual void TakeDmg(int dmg, DmgType type)
    {
        // Debug.Log($"{gameObject.name} {dmg} {type}");
        if (state == BlockState.Inactive)
        {
            return;
        }
        //no dmg if player is 
        if (isControlledByPlayer && type == DmgType.Player) return;

        //no dmg if non player is vs enemy
        // if (!isControlledByPlayer && type == DmgType.Enemy) return;

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
        if (!collider.TryGetComponent(out Cube otherCube)) return;

        if (otherCube.isControlledByPlayer)
        {
            // attempt to connect cube to player
            float playerRotation = otherCube.controllingConstruction.transform.eulerAngles.z;
            Vector2Int direction = GetDirection(otherCube.transform.position, transform.position, playerRotation);
            if (otherCube.controllingConstruction.AddCube(this, otherCube, direction))
            {
                state = BlockState.Controlled;
                isControlledByPlayer = true;
                controllingConstruction.GetIndexFromCube(this, out Vector2Int index);
                particles.SetActive(false);
                GetComponent<BoxCollider2D>().size = new Vector2(0.95f, 0.95f);
                currentHp = baseHP * 2;
                if (animator != null)
                {
                    animator.SetTrigger("Green");
                }
            }
        }
    }

    public virtual void Disconnect()
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