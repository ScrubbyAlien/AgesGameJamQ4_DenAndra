using System.Collections.Generic;
using UnityEngine;

public class SuggpropLogic : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabs;
    [SerializeField] private Cube suggPropCube;
    public bool grabbed = false;
    public bool isBack = false;
    public bool firedSuggprop = false;
    private Cube stolenCube;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Cube otherCube)) return;

        if (grabbed) return;

        if (otherCube == suggPropCube) return;

            if (suggPropCube.isControlledByPlayer != otherCube.isControlledByPlayer)
        {
            //Run logic to steal;

            Steal(otherCube);
        }
    }

    public void Steal(Cube cube)
    {
        Debug.Log("Stealing");
        cube.Disconnect();
        cube.currentHp = 1000;
        cube.EnterControlledState();
        grabbed = true;
        cube.transform.parent = this.transform;
        stolenCube = cube;
        firedSuggprop = false;

    }

    public void OnIsBack()
    {
        if (isBack) return;
        Debug.Log("Is back");
        isBack = true;
        transform.DetachChildren();
        stolenCube.currentHp = stolenCube.baseHP;
        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        if(grabbed)
        {
            pullback();
        }
    }

    private void pullback()
    {
        var distance = suggPropCube.transform.position - transform.position;

        Debug.Log("Adding force to pull it back");
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.7f, transform.localPosition.z);

        var heading = distance.magnitude;
        if (heading < 0.7f)
        {
            OnIsBack();
        }
    }
}
