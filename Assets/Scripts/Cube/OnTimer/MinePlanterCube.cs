using UnityEngine;
using UnityEngine.Events;

public class MinePlanterCube : Cube
{
    public override string prefabName => "MinePlanter";

    [Header("Canon Cube")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float reloadTime;
    [SerializeField] private Transform shootPostion;
    [SerializeField] private Transform targetPosition;
    private float timer;

    [SerializeField]
    private UnityEvent OnFire;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > reloadTime)
        {
            ThrowMine();
            timer = 0;
        }
    }
    public void ThrowMine()
    {       
        if (state == BlockState.Inactive) return;
        //Debug.Log("Fire");
        GameObject projectileOb = Instantiate(projectilePrefab, shootPostion.position, shootPostion.rotation, null);
        Mine mine  = projectileOb.GetComponent<Mine>();
        if(mine != null)
        {
            OnFire?.Invoke();
            mine.PlanForMine(targetPosition);
        }

    }
}
