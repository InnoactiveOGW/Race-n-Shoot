using UnityEngine;
using System.Collections;

public enum ItemType
{
    None,
    HealthPack,
    AmunitionPack,
    Invincibility,
    SpeedBoost,
    EMP,
    Lightning
};



public class ItemsController : MonoBehaviour
{
    [HideInInspector]
    public ItemType currentItem = ItemType.None;

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

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        float trigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        if (trigger > 0f && currentItem != ItemType.None)
        {
            UseItem(currentItem);
            currentItem = ItemType.None;
        }
    }

    public bool CanCollectItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.HealthPack:
                return playerController.NeedsHealthpack();

            case ItemType.AmunitionPack:
                return playerController.NeedsAmunition();

            default:
                return currentItem == ItemType.None;
        }
    }

    public void CollectItem(ItemType itemType)
    {
        if (itemType == ItemType.HealthPack || itemType == ItemType.AmunitionPack)
        {
            UseItem(itemType);
        }
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
