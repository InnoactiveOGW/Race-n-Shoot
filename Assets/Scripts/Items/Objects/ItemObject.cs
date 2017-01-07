using UnityEngine;
using System.Collections;

public class ItemObject : MonoBehaviour
{
    private ItemsController itemsController;

    [SerializeField]
    private CollectableType collectableType;

    [SerializeField]
    private float respawnTime;
    private float respawnTimer;

    private BoxCollider boxCollider;
    private MeshRenderer meshRenderer;
    private MeshRenderer[] meshRenderers;

    void Awake()
    {
        itemsController = FindObjectOfType<ItemsController>();
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
        }
    }

    void Update()
    {
        if ((meshRenderer != null && meshRenderer.enabled) || (meshRenderers != null && meshRenderers[0].enabled))
            return;

        respawnTimer += Time.deltaTime;
        if (respawnTimer >= respawnTime)
        {
            boxCollider.enabled = true;

            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }
            else
            {
                foreach (MeshRenderer childMeshRenderer in meshRenderers)
                    childMeshRenderer.enabled = true;
            }

            respawnTimer = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        if (itemsController.CanCollectItem(collectableType))
        {
            boxCollider.enabled = false;

            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
            else
            {
                foreach (MeshRenderer childMeshRenderer in meshRenderers)
                    childMeshRenderer.enabled = false;
            }

            itemsController.CollectItem(collectableType);
        }
    }
}
