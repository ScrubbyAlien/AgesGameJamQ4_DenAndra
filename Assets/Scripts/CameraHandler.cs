using Unity.Cinemachine;
using UnityEngine;
public class Camera : MonoBehaviour
{
    [Header("Values for Design")]
    [SerializeField] private float cameraStartSize;
    [SerializeField] private float cameraSizePerBlock;
    [SerializeField] private CubeConstruction cubeConstruct;

    [Header("Dependecies")]
    [SerializeField] private CinemachineCamera camera;


    [Header("Info")]
    [SerializeField] private float cameraSize;
    private int nrOfBlocks; // Might replace with info from other scirpt later on

    private void Awake()
    {
        cubeConstruct = GetComponent<CubeConstruction>();

    }

    private void OnEnable()
    {
        if(cubeConstruct != null)
        {
            cubeConstruct.OnAddCube += UpdateCameraSize;
        }
    }

    private void OnDisable()
    {
        if (cubeConstruct != null)
        {
            cubeConstruct.OnAddCube -= UpdateCameraSize;
        }
    }

    private void Start()
    {
        camera.Lens.OrthographicSize = cameraStartSize;
    }

    public void UpdateCameraSize(int nrOfcubes)
    {
        nrOfBlocks = nrOfcubes;
       camera.Lens.OrthographicSize = cameraStartSize + cameraSizePerBlock * nrOfBlocks;
    }





}