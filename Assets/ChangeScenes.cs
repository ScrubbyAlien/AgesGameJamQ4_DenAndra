using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public UnityEvent PlayTrack;
    public UnityEvent PauseTrack;

    public void OnChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
        PauseTrack.Invoke();
        PlayTrack.Invoke();
        
        
    }
}
