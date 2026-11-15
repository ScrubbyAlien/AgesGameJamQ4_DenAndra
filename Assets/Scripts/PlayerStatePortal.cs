using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatePortal", menuName = "Scriptable Objects/PlayerStatePortal")]
public class PlayerStatePortal : ScriptableObject
{
    public event Action FixStuckness;

    [SerializeField]
    private CubePrefabInfo[] cubePrefabInfos;

    [HideInInspector]
    public Vector2 playerPosition;
    public Dictionary<Vector3Int, string> playerCubeState { get; private set; }
    public bool readPreviousCubeState { get; private set; }

    public void ReadPlayerCubeState(CubeConstruction playerConstruction)
    {
        playerCubeState = playerConstruction.CopyCubeState();
        readPreviousCubeState = true;
    }

    public void RemeberLastCubeState()
    {
        readPreviousCubeState = true;
    }

    public Dictionary<Vector3Int, string> ExtractPreviousCubeState()
    {
        if (readPreviousCubeState)
        {
            readPreviousCubeState = false;
            return playerCubeState;
        }
        return null;
    }

    public Cube GetPrefab(string name)
    {
        return cubePrefabInfos.First(i => i.name == name).prefab;
    }

    public void InvokeFixStuckness()
    {
        FixStuckness?.Invoke();
    }

    [Serializable]
    private struct CubePrefabInfo
    {
        public string name;
        public Cube prefab;
    }
}