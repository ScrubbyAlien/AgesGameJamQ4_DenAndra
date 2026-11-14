using UnityEngine;

public class MinePlanterCube : Cube
{

    [Header("Canon Cube")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float reloadTime;
    [SerializeField] private Transform shootPostion;
    [SerializeField] private Transform targetPosition;
    private float timer;

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
            mine.PlanForMine(targetPosition);
        }

    }
}
