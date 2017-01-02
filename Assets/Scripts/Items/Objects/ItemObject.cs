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

    void Awake()
    {
        itemsController = FindObjectOfType<ItemsController>();
    }

    void Update()
    {
        if (GetComponent<MeshRenderer>().enabled)
            return;

        respawnTimer += Time.deltaTime;
        if (respawnTimer >= respawnTime)
        {
            GetComponent<BoxCollider>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
            respawnTimer = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        if (itemsController.CanCollectItem(collectableType))
        {
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            itemsController.CollectItem(collectableType);
        }
    }
}
