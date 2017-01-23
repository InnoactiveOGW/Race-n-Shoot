using UnityEngine;
using System.Collections;

public class Lightning : ThrowableItem
{
    private GameController gameController;
    private ItemsController itemsController;

    private Rigidbody rigidbody;
    private MeshRenderer renderer;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject lightning;
    [SerializeField]
    private SkinnedMeshRenderer smRenderer;

    [SerializeField]
    private float damageRadius = 20f;
    [SerializeField]
    private float maxDamage = 200f;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponent<MeshRenderer>();

        gameController = FindObjectOfType<GameController>();
        itemsController = FindObjectOfType<ItemsController>();
    }

    public override void Execute(Vector3 position)
    {
        rigidbody.isKinematic = true;
        renderer.enabled = false;

        lightning.transform.parent = null;
        lightning.transform.position = position;
        lightning.transform.rotation = Quaternion.identity;
        lightning.transform.localScale = new Vector3(1, 1, 1);
        smRenderer.SetBlendShapeWeight(0, 100);

        StartCoroutine(ExpandLightning(position));

        itemsController.SetCurrentItem(ItemType.None);
    }

    private IEnumerator ExpandLightning(Vector3 position)
    {
        float elapsedTime = 0.0f;
        float duration = 0.2f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            smRenderer.SetBlendShapeWeight(0, 100 - 100 * t);
            yield return null;
        }

        animator.SetTrigger("Shockwave");

        DamageSurroundingEnemies(position);

        Destroy(gameObject, 0.5f);
        Destroy(lightning, 0.5f);
    }

    private void DamageSurroundingEnemies(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, damageRadius);
        foreach (Collider collider in colliders)
        {
            Health health = collider.GetComponent<Health>();
            if (health == null)
                continue;

            float distance = Vector3.Distance(collider.transform.position, position);
            if (distance > damageRadius)
            {
                health.TakeDamage(maxDamage);
                continue;
            }

            float relativeDistance = (damageRadius - distance) / damageRadius;
            float damage = relativeDistance * maxDamage;
            health.TakeDamage(damage);
        }
    }
}
