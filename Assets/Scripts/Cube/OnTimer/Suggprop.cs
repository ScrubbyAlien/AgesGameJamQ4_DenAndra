using UnityEngine;
using static UnityEngine.UI.Image;

public class SuggpropCube : Cube 
{
    public override string prefabName => "SuggproprCube";

    [Header("Suggprop")]
    [SerializeField] private Transform suggprop;
    [SerializeField] private Transform raycastStart;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private SuggpropLogic suggpropLogic;
    private bool firedSuggprop;

    private void FixedUpdate()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, suggprop.position);

        RaycastHit2D hit = Physics2D.Raycast(raycastStart.position, -Vector2.up);
        if (hit.collider != null)
        {
            Debug.DrawRay(raycastStart.position, -transform.up * 20f, Color.red);
            if(!hit.collider.TryGetComponent(out Cube otherCube)) return;

            if (otherCube.isControlledByPlayer)
            {
                ShootSuggProp();
            }

        }
    }

    private void ShootSuggProp()
    {
        if (firedSuggprop) return;
        firedSuggprop = true;
        suggpropLogic.FireSuggprop();
    }
}
