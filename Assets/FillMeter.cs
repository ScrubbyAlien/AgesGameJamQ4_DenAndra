using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class FillMeter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] Image fillImage;
    [SerializeField] EnemySpawner spawner;
    private int killCount;
    private float amountToFill;
    private float maxAmount = 1;
    
    
    
    
    void Start()
    {
        CoreCube.EnemyDied += IncrementKillCount;
        CalculateEnemies();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CalculateEnemies()
    {
        amountToFill = maxAmount / spawner.enemiesInWave;
        Debug.Log("OLLE" + spawner.enemiesInWave);
        Debug.Log("OLLE" + amountToFill);
        Debug.Log("OLLE" + maxAmount);

    }

    public void ResetFill(EnemySpawner newSpawner)
    {
        fillImage.fillAmount = 0;
        spawner = newSpawner;
        CalculateEnemies();
    }

    private void IncrementKillCount()
    {
        killCount++;
        fillImage.fillAmount += amountToFill;
        
    }

    private void OnDestroy()
    {
        CoreCube.EnemyDied -= IncrementKillCount;
    }
    
    
}
