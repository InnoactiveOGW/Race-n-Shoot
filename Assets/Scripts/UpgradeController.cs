using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradeController : MonoBehaviour
{
    private GameController gameController;
    private PlayerController playerController;

    [SerializeField]
    private int numOfUpgrades = 3;
    [HideInInspector]
    public bool hasAllUpgrades;
    private List<Upgrade> appliedUpgrades;

    private Upgrade selectedUpgrade;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        playerController = FindObjectOfType<PlayerController>();

        appliedUpgrades = new List<Upgrade>();
        hasAllUpgrades = false;
    }

    void Update()
    {
        if (selectedUpgrade != null && OVRInput.Get(OVRInput.Button.One))
        {
            ApplyUpgrade();
        }
    }

    public void SelectUpgrade(Upgrade upgrade)
    {
        if (appliedUpgrades.Contains(upgrade))
            return;

        if (selectedUpgrade != null)
        {
            selectedUpgrade.Deselect();
        }

        selectedUpgrade = upgrade;
        selectedUpgrade.Select();
    }

    private void ApplyUpgrade()
    {
        switch (selectedUpgrade.index)
        {
            case 0:
                playerController.ApplyHealthUpgrade();
                break;

            case 1:
                gameController.ShowAmunition();
                break;

            case 2:
                playerController.ApplyDoubleFireUpgrade();
                break;

            default:
                return;
        }

        appliedUpgrades.Add(selectedUpgrade);
        if (appliedUpgrades.Count == numOfUpgrades)
            hasAllUpgrades = true;

        gameController.UpgradeApplied();
        selectedUpgrade = null;
    }
}
