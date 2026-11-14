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

    [SerializeField]
    private GameObject disconnectedConstructionPrefab;

    private void Start()
    {
        cubes = new();
        if (transform.childCount > 0)
        {
            Cube coreCube = transform.GetChild(0).GetComponent<Cube>();
            if (coreCube)
            {
                AddInitialCube(coreCube);
            }

            for (int i = 1; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out Cube childCube))
                {
                    // transform position better be fucking correct istg
                    Vector2Int place = Vector2Int.RoundToInt((Vector2)childCube.transform.localPosition);
                    AddCubeAtPosition(childCube, place);
                }
            }
        }
    }

    private void AddInitialCube(Cube startCube)
    {
        AddCubeAtPosition(startCube, Vector2Int.zero);
    }

    private void AddCubeAtPosition(Cube cubeToAdd, Vector2Int position)
    {
        cubes.Add(position, cubeToAdd);
        cubeToAdd.controllingConstruction = this;
        cubeToAdd.positionInConstruction = position;
        cubeToAdd.isControlledByPlayer = GetComponent<PlayerController>();
        cubeToAdd.state = BlockState.Controlled;
        OnAddCube?.Invoke(count);
    }

    public bool AddCube(Cube cubeToAdd, Cube cubeToAddTo, Vector2Int direction)
    {
        if (!GetIndexFromCube(cubeToAddTo, out Vector2Int index)) return false; // cube to add to doesn't exist
        Vector2Int newIndex = index + direction;
        if (cubes.ContainsKey(newIndex)) return false; // place to add to is already occupied

        cubeToAdd.controllingConstruction?.RemoveCube(cubeToAdd);
        cubes.Add(newIndex, cubeToAdd); // add cube
        cubeToAdd.transform.parent = transform;
        cubeToAdd.transform.localPosition = IndexToLocalPosition(newIndex);
        float rotation = cubeToAdd.transform.localEulerAngles.z;
        rotation = Mathf.Round(rotation / 90f) * 90f; // round to nearest 90
        cubeToAdd.transform.localEulerAngles = new Vector3(0, 0, rotation);
        cubeToAdd.controllingConstruction = this;
        cubeToAdd.isControlledByPlayer = GetComponent<PlayerController>();
        cubeToAdd.state = BlockState.Controlled;
        OnAddCube?.Invoke(count);

        return true;
    }

    public void RemoveCube(Cube cubeToRemove)
    {
        if (GetIndexFromCube(cubeToRemove, out Vector2Int removeIndex))
        {
            if (removeIndex == Vector2Int.zero)
            {
                Disintigrate();
                return;
            }
            cubes.Remove(removeIndex);
        }

        List<List<Cube>> clusters = GetDistinctClusters();
        List<Cube> coreCluster = clusters[0];

        for (int i = 1; i < clusters.Count; i++)
        {
            CreateNewConstructionFromCluster(clusters[i]);
        }
    }

    private void Disintigrate()
    {
        foreach (Cube cube in cubes.Values)
        {
            cube.transform.parent = null;
            cube.controllingConstruction = null;
            cube.positionInConstruction = Vector2Int.zero;
            cube.isControlledByPlayer = false;
            cube.state = BlockState.Inactive;
        }
        cubes.Clear();
        Destroy(gameObject);
    }

    public void CreateNewConstructionFromCluster(List<Cube> cluster)
    {
        Vector2 position = cluster[0].transform.position;
        Quaternion rotation = cluster[0].transform.rotation;
        GameObject newObject = Instantiate(disconnectedConstructionPrefab, position, rotation);
        foreach (Cube cube in cluster)
        {
            cube.transform.parent = newObject.transform;
            GetIndexFromCube(cube, out Vector2Int index);
            cubes.Remove(index);
        }
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
        return FloodFillCluster(Vector2Int.zero);
    }

    private List<List<Cube>> GetDistinctClusters()
    {
        List<List<Cube>> cubeClusters = new();
        List<Cube> allCubes = cubes.Values.ToList();
        List<Cube> coreCluster = CubesConnectedToCore();
        cubeClusters.Add(coreCluster);
        allCubes.RemoveAll(c => coreCluster.Contains(c));
        while (allCubes.Count > 0)
        {
            GetIndexFromCube(allCubes[0], out Vector2Int index);
            List<Cube> newCluster = FloodFillCluster(index);
            Debug.Log(newCluster.Count);
            allCubes.RemoveAll(c => newCluster.Contains(c));
            cubeClusters.Add(newCluster);
        }

        return cubeClusters;
    }

    private List<Cube> FloodFillCluster(Vector2Int start)
    {
        List<Cube> connectedCubes = new();
        if (cubes.Count == 0) return connectedCubes;
        connectedCubes.Add(cubes[start]);
        HashSet<Vector2Int> vistited = new();
        Queue<Vector2Int> frontier = new();
        frontier.Enqueue(start);

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