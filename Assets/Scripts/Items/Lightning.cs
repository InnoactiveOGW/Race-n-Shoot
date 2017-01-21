using UnityEngine;
using System.Collections;

public class Lightning : ThrowableItem
{
    private GameController gameController;
    private ItemsController itemsController;

    [SerializeField]
    private Rigidbody rigidbody;
    [SerializeField]
    private MeshRenderer renderer;
    [SerializeField]
    private GameObject lightning;
    [SerializeField]
    private SkinnedMeshRenderer smRenderer;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float damageRadius = 10f;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        itemsController = FindObjectOfType<ItemsController>();
    }

    public override void Execute(Vector3 position)
    {
        Debug.Log("Execute Lightning");
        rigidbody.isKinematic = true;
        renderer.enabled = false;

        lightning.transform.parent = null;
        lightning.transform.position = position;
        lightning.transform.rotation = Quaternion.identity;
        lightning.transform.localScale = new Vector3(1, 1, 1);
        smRenderer.SetBlendShapeWeight(0, 100);

        StartCoroutine(ExpandLightning(position));

        itemsController.currentItem = ItemType.None;
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
        Collider[] colliders = Physics.OverlapSphere(position, );

    }
}
