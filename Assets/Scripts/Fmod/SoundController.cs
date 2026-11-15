using System;
using FMOD.Studio;
using FMODUnity;
using UnityEditor.Rendering;
using UnityEngine;
//Funktionerna av intresse här är PlayOneshot, vilket har en parameter "path" som bestämmer
// vilket fmod-event som kommer spelas. Dessa paths går att läsa från string-listorna
// weaponPath, utilityPath och generalPath. Ljuden kommer spelas från de koordinater på det objekt
//som detta script är satt på. 
//Externa script som ska kontrollera detta bör enbart använda PlayOneshot() för ljud som ska triggas
// en gång. Det enda kontinuerliga ljuden vi har är när kuberna är i rörelse och/eller "thrustar".
//Om detta script sätts på en core-kub,så ska boolen "isCoreBlock" vara sann.
//Boolen "thrustEnabled" ska vara sann då ett thruster-block finns anslutet till core, och det används.
//"initiateContEventsOnStart" boolen, skapar en instans för movement-ljud, som kallar på InitEvent().
//Detta måste göras innan metoderna Play-, Pause-, Stop-, Set-Event metoderna kan användas.
//Obs! StopEvent() frigör eventinstansen från minnet, och kallas bara på när Core-block tas bort/eller kallar på Destroy().
//Använd PauseEvent() när movement ljud ska sluta spelas.
public class SoundController : MonoBehaviour
{
    private GameObject thisObject;
    private EventInstance eventInstance;
    private EventReference eventReference;

    [Header("Local Sound Events")] 
    //För Oneshot (local)
    public EventReference[] oneShotRef;

    [Header("MovementSettings")] 
    [Tooltip("Toggla denna om detta script sätts på en 'Core'")]public bool isCoreBlock;
    [Tooltip("Togglas externt, om core har thruster och körs")]public bool thrustEnabled;
    [Tooltip("Toggla denna om Event instanserna ska skapa vid Start")]public bool initiateContEventsOnStart;
    
    [Header("MovementDebug")]
    public bool debugEnabled;
    public bool play;
    public bool pause;
    public bool stop;

    [Header("OneshotDebug")] 
    public int oneshotIndex;
    public bool playOne;
    
    //Kallas med PlayOneshot()
   [HideInInspector] public string[] weaponPath =
    {
        "event:/Sound/Weapons/Pistol",
        "event:/Sound/Weapons/Shotgun",
        "event:/Sound/Weapons/Laser",
        "event:/Sound/Weapons/Rocket",
        "event:/Sound/Weapons/Bomb",
    };
    //Kallas med PlayOneshot()
    [HideInInspector] public string[] utilityPath =
    {
        "event:/Sound/Utility/Sugpropp"
    };

    //Kallas med PlayOneshot()
    [HideInInspector] public string[] generalPath =
    {
        "event:/Sound/General/BlockAdd",
        "event:/Sound/General/BlockRemoved",
        "event:/Sound/General/BlockDamaged",
        "event:/Sound/General/BlockDestroyed",
        "event:/Sound/General/Death",
        "event:/Sound/General/Spin",
        "event:/Sound/General/Fart",
    };
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventReference = RuntimeManager.PathToEventReference("event:/Sound/Movement/Moving");
        
        if (initiateContEventsOnStart)
        {
           InitEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (debugEnabled) //Basic debugging vid runtime.
        {
            if (play)
            {
                PlayEvent();
                play = false;
            }

            if (pause)
            {
                PauseEvent();
                pause = false;
            }

            if (stop)
            {
                StopEvent();
                stop = false;
            }

            if (playOne)
            {
                PlayLocalOneshot(oneshotIndex);
                playOne = false;
            }
        }

        if (isCoreBlock)
        {
            Rigidbody2D rig2D;
            rig2D = GetComponent<Rigidbody2D>();
            RuntimeManager.AttachInstanceToGameObject(eventInstance, gameObject, rig2D);
            CheckThrust();
        }
        
    }

    public void PlayOneshot(string path)
    {
        EventReference evRef;
        evRef = RuntimeManager.PathToEventReference(path);
        RuntimeManager.PlayOneShotAttached(evRef, gameObject);
    }
    
    public void PlayLocalOneshot(int index)
    {
        RuntimeManager.PlayOneShotAttached(oneShotRef[index], gameObject);
    }

    public void PlayEvent()
    {
        eventInstance.getPaused(out bool paused);
        if (paused)
        {
            eventInstance.setPaused(false);
        }
        else
        {
            eventInstance.start();
        }
    }

    public void PauseEvent()
    {
        eventInstance.setPaused(true);
    }

    public void StopEvent()
    {
        eventInstance.release();
        eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void SetEventParam(string name, float value)
    {
        eventInstance.setParameterByName(name, value, false);
    }

    public void InitEvent()
    {
        eventInstance = RuntimeManager.CreateInstance(eventReference);
    }

    private void CheckThrust()
    {
        eventInstance.getParameterByName("Thrust", out float val);
        if (thrustEnabled && val == 0)
        {
            SetEventParam("Thrust", 1);
        }
        else if (!thrustEnabled && val == 1) 
        {
            SetEventParam("Thrust", 0);
        }
    }

    void OnDestroy()
    {
        StopEvent();
    }
}
