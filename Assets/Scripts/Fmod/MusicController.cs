using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private GameObject aM;
    private AudioManager audioManager;
    private EventInstance mInstance;
    
    [Serializable]public struct Param
    {
        public string Name;
        public float Value;
    }
    
    [Header("Music Event")] public int indexRef;
    
    [Header("Event Parameters")]
    public Param[] EventParam;

    [Header("Settings")]
    public bool initOnStart;
    public bool playOnStart;
    public bool releaseOnStop;

    [Header("Debug/Runtime")] 
    public bool debugEnabled;
    public bool play;
    public bool pause;
    public bool stop;
    public int paramIndex;
    public bool setParameter;
    public bool instantiate;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aM = GameObject.FindGameObjectWithTag("AudioManager");
        audioManager = aM.GetComponent<AudioManager>();

         if (initOnStart)
         {
             InitEvent();
         }

         if (playOnStart)
         {
             PlayEvent();
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

            if (setParameter)
            {
                SetEventParam(paramIndex);
                setParameter = false;
            }

            if (instantiate)
            {
                InitEvent();
                instantiate = false;
            }
        }
    }

    public void InitEvent()
    {
        //Skapar FMOD instans med eventreferens från listan muRef
        mInstance = RuntimeManager.CreateInstance(audioManager.muRef[indexRef]);
    }

    public void PlayEvent()
    {
        //Spelar det instantierade eventet
        mInstance.getPaused(out bool paused);
        if (paused)
        {
            mInstance.setPaused(false);
        }
        else
        {
            mInstance.start();
        }
    }

    public void PauseEvent()
    {
        //Pausar event
        mInstance.setPaused(true);
    }

    public void StopEvent()
    {
        //Stoppar event playback.
        if (releaseOnStop)
        {
            //Om sant, så frigörs eventinstansen från minnnet vid "stop".
            mInstance.release();
        }
        
        mInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void SetEventParam(int index)
    {
        //Ändrar parameter i instansens valda eventet , namn och värde.
        mInstance.setParameterByName(EventParam[index].Name, EventParam[index].Value, false);
    }
}
