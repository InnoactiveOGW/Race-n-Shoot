using UnityEngine;
using System.Collections;


public class Upgrade : MonoBehaviour
{
    private UpgradeController upgradeController;
    private AudioSource selectionSound;

    [SerializeField]
    private Collider leftIndex;
    [SerializeField]
    private Collider rightIndex;

    public int index;
    public GameObject upgrade;
    public GameObject image;
    public GameObject imageSelected;

    void Awake()
    {
        upgradeController = FindObjectOfType<UpgradeController>();
        selectionSound = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == leftIndex)
        {
            bool handClosed = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0;
            bool indexPointing = !OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger) && !OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger);
            if (!handClosed || !indexPointing)
                return;
        }
        else if (other == rightIndex)
        {
            bool handClosed = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0;
            bool indexPointing = !OVRInput.Get(OVRInput.Touch.SecondaryIndexTrigger) && !OVRInput.Get(OVRInput.NearTouch.SecondaryIndexTrigger);
            if (!handClosed || !indexPointing)
                return;
        }
        else
        {
            return;
        }

        upgradeController.SelectUpgrade(this);
    }

    public virtual void Select()
    {
        selectionSound.Play();
        upgrade.SetActive(true);
        image.SetActive(false);
        imageSelected.SetActive(true);
    }

    public virtual void Deselect()
    {
        upgrade.SetActive(false);
        imageSelected.SetActive(false);
        image.SetActive(true);
    }

}
