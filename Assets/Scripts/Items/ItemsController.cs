using UnityEngine;
using System.Collections;

public enum CollectableType
{
    Random,
    HealthPack,
    AmunitionPack,
    Count
};

public enum ItemType
{
    None,
    HealthPack,
    AmunitionPack,
    Invincibility,
    SpeedBoost,
    Count
};



public class ItemsController : MonoBehaviour
{
    private ItemType currentItem = ItemType.None;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private float invincibilityDuration;
    [SerializeField]
    private float speedBoostMultiplicator;
    [SerializeField]
    private float speedBoostDuration;

    void Update()
    {
        if (Input.GetButton("Fire1") && currentItem != ItemType.None)
        {
            UseItem(currentItem);
            currentItem = ItemType.None;
        }
    }

    public bool CanCollectItem(CollectableType collectableType)
    {
        switch (collectableType)
        {
            case CollectableType.HealthPack:
                return playerController.NeedsHealthpack();

            default:
                return currentItem == ItemType.None;
        }
    }

    public void CollectItem(CollectableType collectableType)
    {
        ItemType itemType = ItemType.None;
        switch (collectableType)
        {
            case CollectableType.Random:
                int i = Random.Range(3, (int)ItemType.Count);
                itemType = (ItemType)i;
                break;

            case CollectableType.HealthPack:
                itemType = ItemType.HealthPack;
                break;

            case CollectableType.AmunitionPack:
                itemType = ItemType.AmunitionPack;
                break;
        }

        if (collectableType != CollectableType.Random)
            UseItem(itemType);
        else
            currentItem = itemType;
    }

    public void UseItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.HealthPack:
                playerController.UseHealthPack();
                break;

            case ItemType.Invincibility:
                playerController.UseInvincibility(invincibilityDuration);
                break;

            case ItemType.SpeedBoost:
                playerController.UseSpeedBoost(speedBoostDuration, speedBoostMultiplicator);
                break;

            case ItemType.AmunitionPack:
                playerController.UseAmunitionPack();
                break;

            default:
                break;
        }
    }
}
