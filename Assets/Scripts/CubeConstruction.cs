using System;
using System.Collections.Generic;
using UnityEngine;

public class CubeConstruction : MonoBehaviour
{
    public event Action<int> OnAddCube;

    [SerializeField]
    private float cubeSideLength = 1;
    private Dictionary<Vector2Int, Cube> cubes;
    public int count => cubes.Count;

    public bool noOriginCube => !cubes.ContainsKey(Vector2Int.zero) || !cubes[Vector2Int.zero];

    private void Start()
    {
        cubes = new();
        Cube coreCube = transform.GetChild(0).GetComponent<Cube>();
        if (coreCube)
        {
            cubes.Add(Vector2Int.zero, coreCube);
            OnAddCube?.Invoke(count);
            coreCube.controllingConstruction = this;
        }

        for (int i = 1; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Cube childCube))
            {
                // transform position better be fucking correct istg
                Vector2Int place = Vector2Int.RoundToInt((Vector2)childCube.transform.position);
                cubes.Add(place, childCube);
            }
        }
    }

    public bool AddCube(Cube cubeToAdd, Cube cubeToAddTo, Vector2Int direction)
    {
        if (!GetIndexFromCube(cubeToAddTo, out Vector2Int index)) return false; // cube to add to doesn't exist
        Vector2Int newIndex = index + direction;
        if (cubes.ContainsKey(newIndex)) return false; // place to add to is already occupied

        cubes.Add(newIndex, cubeToAdd); // add cube
        OnAddCube?.Invoke(count);
        cubeToAdd.transform.parent = transform;
        cubeToAdd.transform.localPosition = IndexToLocalPosition(newIndex);
        float rotation = cubeToAdd.transform.localEulerAngles.z;
        Debug.Log(rotation);
        rotation = Mathf.Round(rotation / 90f) * 90f; // round to nearest 90
        cubeToAdd.transform.localEulerAngles = new Vector3(0, 0, rotation);
        cubeToAdd.controllingConstruction = this;

        return true;
    }

    public bool GetIndexFromCube(Cube cube, out Vector2Int index)
    {
        index = Vector2Int.zero;
        foreach (KeyValuePair<Vector2Int, Cube> pair in cubes)
        {
            if (pair.Value == cube)
            {
                index = pair.Key;
                return true;
            }
        }

        return false;
    }

    private Vector3 IndexToLocalPosition(Vector2Int index)
    {
        return (Vector2)index * cubeSideLength;
    }

    public bool GetCubeFromIndex(Vector2Int index, out Cube cube)
    {
        if (!cubes.TryGetValue(index, out cube)) return false;
        return true;
    }
}