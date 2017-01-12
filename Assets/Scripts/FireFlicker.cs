using UnityEngine;
using System.Collections;

public class FireFlicker : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer fire;

    void OnEnable()
    {
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            fire.SetBlendShapeWeight(0, Random.Range(0, 100));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
