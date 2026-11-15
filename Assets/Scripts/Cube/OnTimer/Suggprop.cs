using UnityEngine;

public class SuggpropCube : Cube 
{
    public override string prefabName => "SuggproprCube";

    [Header("Suggprop")]
    [SerializeField] private Transform suggprop;
    [SerializeField] private Transform raycastStart;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private SuggpropLogic suggpropLogic;
    

    private void FixedUpdate()
    {
        
        if(suggprop != null)
        {

            if (suggpropLogic.firedSuggprop)
            {

                suggprop.localPosition = new Vector3(suggprop.localPosition.x, suggprop.localPosition.y + -0.5f, suggprop.localPosition.z);

            }
        }
        

        lineRenderer.SetPosition(0, transform.position);
        if(suggprop != null)
        {
            lineRenderer.SetPosition(1, suggprop.position);

        }
        else
        {
            lineRenderer.SetPosition(1, transform.position);
        }


        RaycastHit2D hit = Physics2D.Raycast(raycastStart.position, -transform.up);
        if (hit.collider != null)
        {
            Debug.DrawRay(raycastStart.position, -transform.up * 20f, Color.red);
            if(!hit.collider.TryGetComponent(out Cube otherCube)) return;

            if (isControlledByPlayer != otherCube.isControlledByPlayer)
            {
                ShootSuggProp();
            }
        }

        
    }

    private void ShootSuggProp()
    {
        if (suggprop != null)
        {
            if (suggpropLogic.firedSuggprop) return;
            suggpropLogic.firedSuggprop = true;
            FireSuggprop();
        }
       
    }

    public void FireSuggprop()
    {
        if (suggprop != null)
        {
            if (!suggpropLogic.grabbed)
            {
                Debug.Log("adding froce to suggprop fire");

                
            }
        }
    }

}
