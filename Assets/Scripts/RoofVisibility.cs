using UnityEngine;
using System.Collections;

public class RoofVisibility : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private SkinnedMeshRenderer roofRenderer;

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, player.transform.position - Camera.main.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            if (hit.collider.gameObject == this.gameObject)
                HideRoof();
            else
                ShowRoof();
        }
    }

    private void HideRoof()
    {
        if (roofRenderer != null)
            SetBlendShapesWeightTo(100);
    }

    private void ShowRoof()
    {
        if (roofRenderer != null)
            SetBlendShapesWeightTo(0);
    }

    private void SetBlendShapesWeightTo(int weight)
    {
        for (int i = 0; i < roofRenderer.sharedMesh.blendShapeCount; i++)
            roofRenderer.SetBlendShapeWeight(i, weight);
    }
}
