using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text counterText;
    [SerializeField]
    private CubeConstruction construction;

    private void Start()
    {
        construction.OnAddCube += UpdateCounter;
        counterText.text = $"{construction.count}";
    }

    private void UpdateCounter(int newCount)
    {
        counterText.text = $"{newCount}";
    }
}