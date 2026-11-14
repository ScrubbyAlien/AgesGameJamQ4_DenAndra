using System;
using FMOD.Studio;
using FMODUnity;
using UnityEditor.Rendering;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private GameObject thisObject;
    private EventInstance eventInstance;
    private EventReference eventReference;
    
    [Serializable]public struct Param
    {
        public string Name;
        public float Value;
    }

    [Header("Local Sound Events")] 
    //För Oneshot (local)
    public EventReference[] oneShotRef;

    [Header("MovementSettings")] 
    [Tooltip("Toggla denna om detta script sätts på en 'Core'")]public bool isCoreBlock;
    [Tooltip("Togglas externt, om core har thruster")]public bool thrustEnabled;
    [Tooltip("Toggla denna om Event instanserna ska skapa vid Start")]public bool initiateContEventsOnStart;
    
    [Header("MovementDebug")]
    public bool debugEnabled;
    public bool play;
    public bool pause;
    public bool stop;
    
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
        }

        if (isCoreBlock)
        {
            Rigidbody2D rig2D;
            rig2D = GetComponent<Rigidbody2D>();
            RuntimeManager.AttachInstanceToGameObject(eventInstance, gameObject, rig2D);
            checkThrust();
        }
        
    }

    public void PlayOneshot(string path)
    {
        EventReference evRef;
        evRef = EventReference.Find(path);
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

    private void checkThrust()
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
