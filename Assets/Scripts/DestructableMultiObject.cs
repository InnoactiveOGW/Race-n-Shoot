using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DestructableMultiObject : Health
{
    [SerializeField]
    private BoxCollider boxCollider;

    [SerializeField]
    private SkinnedMeshRenderer[] smRenderers;

    [SerializeField]
    private Animator animator;

    public override void SetHealthUI()
    {
        if (smRenderers.Length == 0)
            return;

        foreach (SkinnedMeshRenderer smRenderer in smRenderers)
        {
            smRenderer.SetBlendShapeWeight(0, (1 - (currentHealth / startingHealth)) * 100);
        }
    }

    public override void OnDeath()
    {
        animator.SetTrigger("Die");
        boxCollider.enabled = false;
    }
}
