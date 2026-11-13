using Unity.VisualScripting;
using UnityEngine;

public abstract class Cube : MonoBehaviour
{
    public abstract BlockState state {  get; set; }
    public abstract int hp { get; set;}
    public abstract int collsionDmg{ get; set; }

    public virtual void OnCollisionEnter2D(Collision2D collider)
    {
        if(state == BlockState.Inactive)
        {
            Connect(collider);
        }
    }
    public virtual void TakeDmg(int dmg)
    {
        if(state == BlockState.Inactive)
        {
            return;
        }

        hp -= dmg;
        if(hp <= 0)
        {
            hp = 0;
            state = BlockState.Inactive;
        }
        
    }
    public virtual void OnTimer()
    {
        if(state == BlockState.Inactive)
        {
            return;
        }
    }

    //Handle Connection
    public virtual void Connect(Collision2D collider)   
    {
        if(state != BlockState.Inactive)
        {
            return;
        }
    } 
}

public enum BlockState
{
    Inactive,
    Controlled
}
