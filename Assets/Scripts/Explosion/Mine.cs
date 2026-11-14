using Unity.VisualScripting;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timeUntillActivation = 0.5f;
    private float timer = 0;
    private bool mineActivated = false;
    private bool blownUp = false;
    [SerializeField] private GameObject explosion;
    public void PlanForMine(Transform targetLocation)
    {
        Vector3 direction = (targetLocation.position - transform.position).normalized;

        transform.position += direction * speed;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (mineActivated)
        {
            if (blownUp) return;

            if (!collision.TryGetComponent(out Cube otherCube)) return;
            

            BlowUp();
        }
    }

    private void Update()
    {
        if (mineActivated) return;
        timer += Time.deltaTime;
        if (timer > timeUntillActivation)
        {
            ActivateMine();
        }
    }

    private void ActivateMine()
    {
        mineActivated = true;


    }

    private void BlowUp()
    {
        blownUp = true;
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
