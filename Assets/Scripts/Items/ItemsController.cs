using UnityEngine;
using System.Collections;

public enum ItemType
{
    HealthPack
};

public class ItemsController : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth playerHealth;

    public bool CanCollectItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.HealthPack:
                return playerHealth.currentHealth < playerHealth.startingHealth;

            default:
                return false;
        }
    }

    public void CollectItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.HealthPack:
                UseHealthPack();
                return;

            default:
                return;
        }
    }

    private void UseHealthPack()
    {
        Debug.Log("UseHealthPack");
        playerHealth.ResetHealth();
    }
}
