using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FireAnimationObject : DestructionAnimationObject
{
    [SerializeField]
    private SkinnedMeshRenderer[] fireObjects;
    [SerializeField]
    private float[] fireDelayTimes;

    public override void OnDeath()
    {
        base.OnDeath();

        int index = 0;
        foreach (SkinnedMeshRenderer fire in fireObjects)
        {
            StartCoroutine(SetActiveAfter(fire.gameObject, fireDelayTimes[index]));
            StartCoroutine(FireFlicker(fire));

            index += 1;
        }
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