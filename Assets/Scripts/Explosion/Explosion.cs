using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 0.1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("FUCK ME");

        if (!collision.TryGetComponent(out Cube otherCube)) return;
        otherCube.TakeDmg(1000, DmgType.Neutral);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("FUCK ME");

        if (!collision.TryGetComponent(out Cube otherCube)) return;
        otherCube.TakeDmg(1000, DmgType.Neutral);
    }
}
