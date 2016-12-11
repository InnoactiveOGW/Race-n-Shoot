using UnityEngine;
using UnityEngine.UI;

public class DestructableObject : Health
{
    [SerializeField]
    private SkinnedMeshRenderer renderer;

    public override void SetHealthUI()
    {
        renderer.SetBlendShapeWeight(0, (1 - (currentHealth / startingHealth)) * 100);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        gameObject.SetActive(false);
    }
}
