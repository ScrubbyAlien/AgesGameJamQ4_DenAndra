using System;
using System.Collections.Generic;
using System.Linq;
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
                childCube.controllingConstruction = this;
                childCube.positionInConstruction = place;
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

    public void RemoveCube(Cube cubeToRemove)
    {
        if (GetIndexFromCube(cubeToRemove, out Vector2Int index))
        {
            cubes.Remove(index);
        }
        Debug.Log(CubesConnectedToCore().Count);
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

    public Dictionary<Type, int> GetCubeTypes()
    {
        Dictionary<Type, int> types = new();
        foreach (Cube cube in cubes.Values)
        {
            Type cubeType = cube.GetType();
            if (types.TryGetValue(cubeType, out int count))
            {
                types[cubeType] = count + 1;
            }
            else
            {
                types.Add(cubeType, 1);
            }
        }
        return types;
    }

    public List<Cube> CubesConnectedToCore()
    {
        List<Cube> connectedCubes = new() { cubes[Vector2Int.zero] };
        HashSet<Vector2Int> vistited = new();
        Queue<Vector2Int> frontier = new();
        frontier.Enqueue(Vector2Int.zero);

        while (frontier.Count > 0)
        {
            Vector2Int next = frontier.Dequeue();
            vistited.Add(next);
            Vector2Int[] neighbours = Neighbours(next);

            foreach (Vector2Int neighbour in neighbours)
            {
                if (cubes.ContainsKey(neighbour) && !vistited.Contains(neighbour))
                {
                    connectedCubes.Add(cubes[neighbour]);
                    if (!frontier.Contains(neighbour)) frontier.Enqueue(neighbour);
                }
            }
        }

        return connectedCubes;
    }

    private Vector2Int[] Neighbours(Vector2Int origin)
    {
        return new[] {
            origin + Vector2Int.up,
            origin + Vector2Int.down,
            origin + Vector2Int.left,
            origin + Vector2Int.right,
        };
    }
}