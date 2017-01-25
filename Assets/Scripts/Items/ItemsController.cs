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
    private GameController gameController;
    private PlayerController playerController;

    [SerializeField]
    private Renderer lightRenderer;
    private Color[] lightColors;

    [HideInInspector]
    public ItemType currentItem = ItemType.None;

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
        gameController = FindObjectOfType<GameController>();
        playerController = FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        lightColors = new Color[lightRenderer.materials.Length];
        for (int i = 0; i < lightRenderer.materials.Length; i++)
        {
            lightColors[i] = lightRenderer.materials[i].GetColor("_EmissionColor");
        }

        TurnItemLightsOn(false);
    }

    void Update()
    {
        float trigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        if (trigger > 0f && currentItem != ItemType.None)
        {
            UseItem(currentItem);
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
            SetCurrentItem(itemType);
            switch (currentItem)
            {
                case ItemType.EMP:
                    Instantiate(empBall);
                    TurnItemLightsOn(true);
                    break;

                case ItemType.Lightning:
                    Instantiate(lightningBall);
                    TurnItemLightsOn(true);
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
                SetCurrentItem(ItemType.None);
                break;

            case ItemType.AmunitionPack:
                playerController.UseAmunition();
                SetCurrentItem(ItemType.None);
                break;

            case ItemType.Invincibility:
                playerController.UseInvincibility(invincibilityDuration);
                SetCurrentItem(ItemType.None);
                break;

            case ItemType.SpeedBoost:
                playerController.UseSpeedBoost(speedBoostDuration, speedBoostMultiplicator);
                SetCurrentItem(ItemType.None);
                break;

            default:
                break;
        }
    }

    public void SetCurrentItem(ItemType itemType)
    {
        currentItem = itemType;
        string itemText = "";
        switch (itemType)
        {
            case ItemType.Invincibility:
                itemText = "Invincibility";
                break;

            case ItemType.SpeedBoost:
                itemText = "Speed Boost";
                break;

            case ItemType.EMP:
                itemText = "EMP";
                break;

            case ItemType.Lightning:
                itemText = "Lightning";
                break;

            default:
                TurnItemLightsOn(false);
                break;
        }

        gameController.itemText.text = itemText;
    }

    private void TurnItemLightsOn(bool on)
    {
        float intensity = on ? 1.5f : 0f;
        for (int i = 0; i < lightRenderer.materials.Length; i++)
        {
            lightRenderer.materials[i].SetColor("_EmissionColor", lightColors[i] * intensity);
        }
        DynamicGI.UpdateMaterials(lightRenderer);
    }
}
