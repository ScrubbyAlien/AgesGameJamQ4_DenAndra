using UnityEngine;
using UnityEngine.UI;

public class HealthMeter : MonoBehaviour
{
    
    [SerializeField] Image healthBar;
    [SerializeField] Cube player;
    
  
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = player.currentHp / 20f;
       
    }
/*
    private void UpdateMeter(int currentHp)
    {
       
        
        healthBar.fillAmount = currentHp / 20f;
        
    }

    private void OnDestroy()
    {
        CoreCube.PlayerTakeDamage -= UpdateMeter;
    }
    */
}
