using UnityEngine;

public class EventGUID : MonoBehaviour
{
    //Kallas med PlayOneshot()
    public string[] weaponPath =
    {
        "event:/Sound/Weapons/Pistol",
        "event:/Sound/Weapons/Shotgun",
        "event:/Sound/Weapons/Laser",
        "event:/Sound/Weapons/Rocket",
        "event:/Sound/Weapons/Bomb",
    };
    //Kallas med PlayOneshot()
    public string[] utilityPath =
    {
        "event:/Sound/Utility/Sugpropp"
    };

    //Kallas med PlayOneshot()
    public string[] generalPath =
    {
        "event:/Sound/General/BlockAdd",
        "event:/Sound/General/BlockRemoved",
        "event:/Sound/General/BlockDamaged",
        "event:/Sound/General/BlockDestroyed",
        "event:/Sound/General/Death",
    };
    
   
}
