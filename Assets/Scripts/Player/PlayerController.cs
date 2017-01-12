using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private PlayerMovement movement;
    [SerializeField]
    private PlayerHealth health;
    [SerializeField]
    private PlayerGunRotation gunRotation;

    [SerializeField]
    private GameObject singleFireWeapon;
    [SerializeField]
    private GameObject doubleFireWeapon;
    private GameObject gunBarrelEnd;

    [SerializeField]
    private float carTranslationTime;

    void Awake()
    {
        gunBarrelEnd = singleFireWeapon.GetComponentInChildren<MachineGunShooting>().gameObject;
        gunRotation.gunBarrelEnd = gunBarrelEnd.transform;
    }

    public void EnableInteraction(bool enabled)
    {
        characterController.enabled = enabled;
        movement.enabled = enabled;
        health.enabled = enabled;
        gunBarrelEnd.SetActive(enabled);
    }

    public void ResetHealth()
    {
        health.ResetHealth();
    }

    public void MoveTo(Transform transform)
    {
        this.transform.parent = transform;
        StartCoroutine(MoveCar());
    }

    private IEnumerator MoveCar()
    {
        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.localRotation;

        float timeMoved = 0;
        while (timeMoved < carTranslationTime)
        {
            timeMoved += Time.deltaTime;
            transform.transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, timeMoved / carTranslationTime);
            transform.transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, timeMoved / carTranslationTime);
            yield return null;
        }
    }

    // Items


    public bool NeedsHealthpack()
    {
        return health.NeedsHealthpack();
    }

    public void UseHealthPack()
    {
        Debug.Log("UseHealthPack");
        ResetHealth();
    }

    public void UseInvincibility(float duration)
    {
        Debug.Log("UseInvincibility");
        health.invincibilityTimer = duration;
    }

    public void UseSpeedBoost(float duration, float multiplicator)
    {
        Debug.Log("UseSpeedBoost");
        movement.speed = movement.speed * multiplicator;
        movement.speedBoostTimer = duration;
    }

    public void UseAmunitionPack()
    {
        Debug.Log("UseAmunitionPack");
    }

    // Upgrades

    public void ApplyHealthUpgrade()
    {
        health.AddArmor();
    }

    public void ApplyMissileUpgrade()
    {

    }

    public void ApplyDoubleFireUpgrade()
    {
        gunBarrelEnd = doubleFireWeapon.GetComponentInChildren<MachineGunShooting>().gameObject;
        gunRotation.gunBarrelEnd = gunBarrelEnd.transform;
    }

    public void Death()
    {
        EnableInteraction(false);
        gameController.PlayerDied();
    }
}
