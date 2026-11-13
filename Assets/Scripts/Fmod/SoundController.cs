using System;
using FMOD.Studio;
using FMODUnity;
using UnityEditor.Rendering;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private GameObject aM;
    private GameObject thisObject;
    private AudioManager audioManager;
    private EventInstance[] audioInstance;

    [Serializable]public enum Action
    {
        Play,
        Pause,
        Stop,
        SetParameter
    }

    [Serializable] public struct EventControl
    {
        public int contIndex;
        public Action action;
        public Param[] param;
    }

    [Serializable] public struct OneShotStruct
    {
        public EventReference Ref;
        public bool Is3d;
        
    }
    
    [Serializable]public struct Param
    {
        public string Name;
        public float Value;
    }

    [Header("Sound Events")] 
    //För Oneshot
    public OneShotStruct[] OneShot;
    //För ljud som behöver följa gameobject kontinuerlgt.
    public EventReference[] contEvents;
    
    [Header("Event Actions List")]
    public EventControl[] eventControls;

    [Header("Settings")] 
    public bool initiateContEvents;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aM = GameObject.FindGameObjectWithTag("AudioManager");
        audioManager = aM.GetComponent<AudioManager>();

        if (initiateContEvents)
        {
            for (int i = 0; i < contEvents.Length; i++)
            {
                audioInstance[i] = RuntimeManager.CreateInstance(contEvents[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOneshot(int index)
    {
        if (OneShot[index].Is3d)
        {
            RuntimeManager.PlayOneShotAttached(OneShot[index].Ref, gameObject);
        }
        else
        {
            RuntimeManager.PlayOneShot(OneShot[index].Ref);
        }
    }

    public void PlayEvent()
    {
        
    }
    
    
}
