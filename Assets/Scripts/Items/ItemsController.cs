using UnityEngine;
using System.Collections;

public enum ItemType
{
    None,
    HealthPack,
    Invincibility,
    SpeedBoost,
    AmunitionPack
};



public class ItemsController : MonoBehaviour
{
    private ItemType currentItem = ItemType.None;

    [SerializeField]
    private PlayerHealth playerHealth;
    [SerializeField]
    private PlayerMovement playerMovement;


    [SerializeField]
    private float invincibilityDuration;

    [SerializeField]
    private float speedBoostMultiplicator;
    [SerializeField]
    private float speedBoostDuration;

    void Update()
    {
        if (currentItem == ItemType.None)
            return;

        if (Input.GetButton("Fire1"))
        {
            UseCurrentItem();
        }
    }

    public bool CanCollectItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.HealthPack:
                return playerHealth.currentHealth < playerHealth.startingHealth;

            default:
                return currentItem == ItemType.None;
        }
    }

    public void CollectItem(ItemType itemType)
    {
        currentItem = itemType;
        if (itemType == ItemType.HealthPack)
        {
            UseCurrentItem();
        }
    }

    public void UseCurrentItem()
    {
        switch (currentItem)
        {
            case ItemType.HealthPack:
                UseHealthPack();
                break;

            case ItemType.Invincibility:
                UseInvincibility();
                break;

            case ItemType.SpeedBoost:
                UseSpeedBoost();
                break;

            case ItemType.AmunitionPack:
                UseAmunitionPack();
                break;

            default:
                break;
        }

        currentItem = ItemType.None;
    }

    private void UseHealthPack()
    {
        Debug.Log("UseHealthPack");
        playerHealth.ResetHealth();
    }

    private void UseInvincibility()
    {
        Debug.Log("UseInvincibility");
        playerHealth.invincibilityTimer = invincibilityDuration;
    }

    private void UseSpeedBoost()
    {
        Debug.Log("UseSpeedBoost");
        playerMovement.speed = playerMovement.speed * speedBoostMultiplicator;
        playerMovement.speedBoostTimer = speedBoostDuration;
    }

    private void UseAmunitionPack()
    {
        Debug.Log("UseAmunitionPack");
    }
}
