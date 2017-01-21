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
    EMP,
    Lightning,
    Count
};



public class ItemsController : MonoBehaviour
{
    [HideInInspector]
    public ItemType currentItem = ItemType.None;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private float invincibilityDuration;
    [SerializeField]
    private float speedBoostMultiplicator;
    [SerializeField]
    private float speedBoostDuration;

    [SerializeField]
    private GameObject empBall;
    [SerializeField]
    private GameObject lightningBall;

    void Update()
    {
        float trigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        if (trigger > 0f && currentItem != ItemType.None)
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

            case CollectableType.AmunitionPack:
                return playerController.NeedsAmunition();

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
                int i = 6; //Random.Range(3, (int)ItemType.Count);
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
        {
            currentItem = itemType;
            switch (currentItem)
            {
                case ItemType.EMP:
                    Instantiate(empBall);
                    break;

                case ItemType.Lightning:
                    Instantiate(lightningBall);
                    break;

                default:
                    break;
            }
        }
    }

    public void UseItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.HealthPack:
                playerController.UseHealthPack();
                break;

            case ItemType.AmunitionPack:
                playerController.UseAmunition();
                break;

            case ItemType.Invincibility:
                playerController.UseInvincibility(invincibilityDuration);
                break;

            case ItemType.SpeedBoost:
                playerController.UseSpeedBoost(speedBoostDuration, speedBoostMultiplicator);
                break;

            default:
                break;
        }
    }
}
