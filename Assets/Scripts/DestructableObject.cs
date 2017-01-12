using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
        gameObject.SetActive(false);
    }


    public IEnumerator SetActiveAfter(GameObject gameObject, float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(true);
    }

    public IEnumerator FireFlicker(SkinnedMeshRenderer fire)
    {
        while (true)
        {
            fire.SetBlendShapeWeight(0, Random.Range(0, 100));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
