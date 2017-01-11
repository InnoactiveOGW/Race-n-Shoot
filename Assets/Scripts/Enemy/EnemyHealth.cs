using UnityEngine;
using System.Collections;

public class EnemyHealth : DestructableObject
{
    private GameController gameController;

    [SerializeField]
    private GameObject car;
    [SerializeField]
    private GameObject carExplosion;

    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private Color fullHealthColor;
    [SerializeField]
    private Color zeroHealthColor;

    [SerializeField]
    private SkinnedMeshRenderer firstFire;
    [SerializeField]
    private SkinnedMeshRenderer seconedFire;
    [SerializeField]
    private SkinnedMeshRenderer thirdFire;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public override void SetHealthUI()
    {
        base.SetHealthUI();
        _renderer.material.color = Color.Lerp(fullHealthColor, zeroHealthColor, 1 - currentHealth / startingHealth);
    }

    public override void OnDeath()
    {
        gameController.EnemyKilled(10);

        carExplosion.SetActive(true);
        car.SetActive(false);

        carExplosion.GetComponent<Animation>().Play();

        StartCoroutine(FireFlicker(firstFire));
        StartCoroutine(FireFlicker(seconedFire));
        StartCoroutine(FireFlicker(thirdFire));

        StartCoroutine(SetActiveAfter(seconedFire.gameObject, 1.6f));
        StartCoroutine(SetActiveAfter(thirdFire.gameObject, 1.6f));
    }

    private IEnumerator SetActiveAfter(GameObject gameObject, float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(true);
    }

    private IEnumerator FireFlicker(SkinnedMeshRenderer fire)
    {
        while (true)
        {
            fire.SetBlendShapeWeight(0, Random.Range(0, 100));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
