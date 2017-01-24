using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    private Light light;

    [SerializeField]
    private float flickerSpeed = 0.1f;

    [SerializeField]
    private float intensityMin = 0.2f;
    private float intensityMax;

    void Awake()
    {
        light = GetComponent<Light>();
    }

    void Start()
    {
        intensityMax = light.intensity;
        StartCoroutine(DoFlicker());
    }

    private IEnumerator DoFlicker()
    {
        while (true)
        {
            light.intensity = light.intensity == intensityMax ? intensityMin : intensityMax;
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}
