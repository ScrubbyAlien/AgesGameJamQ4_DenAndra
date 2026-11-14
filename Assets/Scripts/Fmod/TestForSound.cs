using FMODUnity;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class TestForSound : MonoBehaviour
{
    public GameObject soundCOnt;
    private SoundController sCont_;
    
    [Header("Weapon Index")]
    [Range(0, 5)]
    public int wIndex;
    public bool wPLay;
    [Header("Utility Index")]
    [Range(0, 1)]
    public int uIndex;
    public bool uPLay;
    [Header("GeneralIndex")]
    [Range(0, 5)]
    public int gIndex;
    public bool gPLay;
    
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

    void Start()
    {
        sCont_ = soundCOnt.GetComponent<SoundController>();
    }

    void Update()
    {
        if (wPLay)
        {
            sCont_.PlayOneshot(weaponPath[wIndex]);
            wPLay = false;
        }

        if (uPLay)
        {
            sCont_.PlayOneshot(utilityPath[uIndex]);
            uPLay = false;
        }

        if (gPLay)
        {
            sCont_.PlayOneshot(generalPath[gIndex]);
            gPLay = false;
        }
    }
    
    
}
