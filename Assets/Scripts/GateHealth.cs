using UnityEngine;
using System.Collections;

public class GateHealth : Health
{
    [SerializeField]
    private SkinnedMeshRenderer[] smRenderers;

    [SerializeField]
    private Animation animation;

    public override void SetHealthUI()
    {
        if (smRenderers.Length > 0)
            return;

        foreach (SkinnedMeshRenderer smRenderer in smRenderers)
        {
            smRenderer.SetBlendShapeWeight(0, (1 - (currentHealth / startingHealth)) * 100);
        }
    }

    public override void OnDeath()
    {
        Debug.Log("gate destroyed");
        animation.Play();
    }
}
