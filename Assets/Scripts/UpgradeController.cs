using UnityEngine;
using System.Collections;

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

    // Use this for initialization
    void Start()
    {
        selectedIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        bool left = Input.GetKeyDown(KeyCode.LeftArrow);
        bool right = Input.GetKeyDown(KeyCode.RightArrow);

        if (left || right)
        {
            int newIndex = selectedIndex + (left ? -1 : 1);
            chooseUpgrade(newIndex);
        }

        if (selectedIndex >= 0 && Input.GetKeyDown(KeyCode.Space))
        {
            applyUpgrade();
        }
    }

    private void chooseUpgrade(int newIndex)
    {
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
        selectedIndex = selectedIndex < 0 ? upgradeImagesSelected.Length - 1 : (selectedIndex >= upgradeImagesSelected.Length ? 0 : selectedIndex);

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
                playerController.ApplyMissileUpgrade();
                break;

            case 2:
                playerController.ApplyDoubleFireUpgrade();
                break;

            default:
                return;
        }

        gameController.UpgradeApplied();
    }
}
