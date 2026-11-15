using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private string nextScene;
    [SerializeField]
    private CubeConstruction playerConstruction;
    [SerializeField]
    private PlayerStatePortal playerStatePortal;

    public void GoToNextScene()
    {
        playerStatePortal.ReadPlayerCubeState(playerConstruction);
        SceneManager.LoadScene(nextScene);
    }

    public void LoadScene(string sceneName)
    {
        playerStatePortal.RemeberLastCubeState();
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneNoMemory(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}