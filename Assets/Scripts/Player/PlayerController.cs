using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private PlayerMovement movement;
    [SerializeField]
    private GameObject gunBarrelEnd;

    public void EnableInteraction(bool enabled)
    {
        characterController.enabled = enabled;
        movement.enabled = enabled;
        gunBarrelEnd.SetActive(enabled);
    }

    public void ApplyHealthUpgrade()
    {

    }

    public void ApplyMissileUpgrade()
    {

    }

    public void ApplyDoubleFireUpgrade()
    {

    }
}
