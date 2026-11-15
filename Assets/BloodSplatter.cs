using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    [SerializeField] GameObject bloodSplatterPrefab;

    public void SpawnBloodSplatter()
    {
        Instantiate(bloodSplatterPrefab, transform.position, Quaternion.identity);
    }
}
