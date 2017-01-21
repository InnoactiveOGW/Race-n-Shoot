using UnityEngine;
using System.Collections;

public class Flicker : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer smRenderer;

    [SerializeField]
    private float flickerSpeed = 0.1f;

    void Start()
    {
        StartCoroutine(DoFlicker());
    }

    private IEnumerator DoFlicker()
    {
        while (true)
        {
            smRenderer.SetBlendShapeWeight(0, Random.Range(0, 100));
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}
