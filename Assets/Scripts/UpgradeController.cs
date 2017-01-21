using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradeController : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private GameObject defaultWeapon;
    [SerializeField]
    private GameObject[] upgrades;
    [SerializeField]
    private GameObject[] upgradeImages;
    [SerializeField]
    private GameObject[] upgradeImagesSelected;

    private int selectedIndex;
    private List<int> appliedUpgrades;

    public bool hasAllUpgrades;

    void Awake()
    {
        appliedUpgrades = new List<int>();
        hasAllUpgrades = false;
    }

    void OnEnable()
    {
        selectedIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        float h = input.y;
        bool left = h < 0;
        bool right = h > 0;

        Debug.Log("left: " + left + " right: " + right);

        if (left || right)
        {
            int newIndex = selectedIndex;

            bool isValidUpgrade = false;
            while (!isValidUpgrade)
            {
                newIndex += left ? -1 : 1;
                newIndex = newIndex < 0 ? upgrades.Length - 1 : (newIndex >= upgrades.Length ? 0 : newIndex);

                if (!appliedUpgrades.Contains(newIndex))
                    isValidUpgrade = true;
            }

            chooseUpgrade(newIndex);
        }

        if (selectedIndex >= 0 && OVRInput.GetDown(OVRInput.Button.One))
        {
            applyUpgrade();
        }
    }

    private void chooseUpgrade(int newIndex)
    {
        if (newIndex == selectedIndex)
            return;

        if (selectedIndex != -1)
        {
            upgrades[selectedIndex].SetActive(false);
            upgradeImagesSelected[selectedIndex].SetActive(false);
            upgradeImages[selectedIndex].SetActive(true);

            if (selectedIndex == 2)
            {
                defaultWeapon.SetActive(true);
            }
        }

        selectedIndex = newIndex;

        if (selectedIndex == 2)
        {
            defaultWeapon.SetActive(false);
        }

        upgradeImages[selectedIndex].SetActive(false);
        upgradeImagesSelected[selectedIndex].SetActive(true);
        upgrades[selectedIndex].SetActive(true);
    }

    private void applyUpgrade()
    {
        switch (selectedIndex)
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

        appliedUpgrades.Add(selectedIndex);
        if (appliedUpgrades.Count == upgrades.Length)
            hasAllUpgrades = true;

        gameController.UpgradeApplied();
    }
}
