using Unity.Cinemachine;
using UnityEngine;
public class Camera : MonoBehaviour
{
    [Header("Values for Design")]
    [SerializeField] private float cameraStartSize;
    [SerializeField] private float cameraSizePerBlock;

    [Header("Dependecies")]
    [SerializeField] private CinemachineCamera camera;


    [Header("Info")]
    [SerializeField] private float cameraSize;
    private int nrOfBlocks; // Might replace with info from other scirpt later on

    public void UpdateCameraSize()
    {
       camera.Lens.OrthographicSize = cameraStartSize + cameraSizePerBlock * nrOfBlocks;
    }





}