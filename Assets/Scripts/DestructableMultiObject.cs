using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DestructableMultiObject : Health
{
    private BoxCollider boxCollider;
    private Animator animator;

    [SerializeField]
    private SkinnedMeshRenderer[] smRenderers;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
    }

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
        base.OnDeath();
        animator.SetTrigger("Die");
        boxCollider.enabled = false;
    }
}
