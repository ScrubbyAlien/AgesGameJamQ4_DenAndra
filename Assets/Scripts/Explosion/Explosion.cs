using UnityEngine;
using UnityEngine.Events;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnExplode;

    private void Start()
    {
        Destroy(gameObject, 0.6f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("FUCK ME");

        if (!collision.TryGetComponent(out Cube otherCube)) return;
        otherCube.TakeDmg(4, DmgType.Neutral);
        OnExplode?.Invoke();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("FUCK ME");

        if (!collision.TryGetComponent(out Cube otherCube)) return;
        otherCube.TakeDmg(4, DmgType.Neutral);
        OnExplode?.Invoke();
    }
}
