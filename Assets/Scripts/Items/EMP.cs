using UnityEngine;
using System.Collections;

public class EMP : ThrowableItem
{
    private GameController gameController;
    private ItemsController itemsController;

    [SerializeField]
    private Rigidbody rigidbody;
    [SerializeField]
    private MeshRenderer renderer;
    [SerializeField]
    private GameObject shockwave;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        itemsController = FindObjectOfType<ItemsController>();
    }

    public override void Execute(Vector3 position)
    {
        Debug.Log("Execute EMP");
        rigidbody.isKinematic = true;
        renderer.enabled = false;
        StartCoroutine(ExpandShockwave());
        gameController.EMP();
        itemsController.currentItem = ItemType.None;
    }

    private IEnumerator ExpandShockwave()
    {
        Vector3 startScale = new Vector3(0.3f, 0.3f, 0.3f);
        Vector3 endScale = new Vector3(15, 15, 15);

        float elapsedTime = 0.0f;
        float duration = 0.8f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            shockwave.transform.localScale = Vector3.Lerp(startScale, endScale, t);

            Renderer renderer = shockwave.GetComponent<MeshRenderer>();
            Color color = renderer.material.color;
            color.a = Mathf.Lerp(0.8f, 0f, t);
            renderer.material.color = color;

            yield return null;
        }

        Destroy(gameObject);
    }
}
