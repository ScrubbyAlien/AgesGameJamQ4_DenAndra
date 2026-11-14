using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Cube otherCube = GetComponent<Cube>();

        if( otherCube != null)
        {
            otherCube.TakeDmg(100, DmgType.Neutral);
        }
    }
}
