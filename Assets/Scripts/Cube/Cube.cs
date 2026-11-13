using Unity.VisualScripting;
using UnityEngine;

public abstract class Cube : MonoBehaviour
{

    public abstract int hp{ get; set; }
    public abstract int collsionDmg{ get; set; }

    public abstract void OnCollisionEnter2D();
    public abstract void TakeDmg();
    public abstract void OnTimer();



}
