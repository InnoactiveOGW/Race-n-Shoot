using UnityEngine;
using UnityEngine.UI;

public class DestructableObject : Health
{
    [SerializeField]
    private SkinnedMeshRenderer smRenderer;

    public override void SetHealthUI()
    {
        if (smRenderer == null)
            return;

        for (int i = 0; i < smRenderer.sharedMesh.blendShapeCount; i++)
        {
            smRenderer.SetBlendShapeWeight(i, (1 - (currentHealth / startingHealth)) * 100);
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();
        gameObject.SetActive(false);
    }
}
