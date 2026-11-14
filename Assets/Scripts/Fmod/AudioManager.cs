using System;
using UnityEngine;
using FMOD.Studio;
using FMOD;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    
    public static AudioManager Instance { get; private set; }
    
    [Header("Music Events")] 
    public EventReference[] muRef;
    
    [Header("Volume")]
    [Range(0f, 1f)]
    public float masterVolume = 1f;
    [Range(0f, 1f)]
    public float effectVolume = 1f;
    [Range(0f, 1f)]
    public float musicVolume = 1f;

    [SerializeField] private string masterVCAPath;
    [SerializeField] private string musicVCAPath;
    [SerializeField] private string soundVCAPath;
    
    private VCA masterVCA;
    private VCA musicVCA;
    private VCA soundVCA;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
        
        soundVCA = RuntimeManager.GetVCA(soundVCAPath);
        masterVCA = RuntimeManager.GetVCA(masterVCAPath);
        musicVCA = RuntimeManager.GetVCA(musicVCAPath);
        
    }

    private void Update()
    {
        masterVCA.setVolume(masterVolume);
        soundVCA.setVolume(effectVolume);
        musicVCA.setVolume(musicVolume);
    }
}
