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

    [SerializeField]
    private Transform missileHolder;
    [SerializeField]
    private GameObject missilePrefab;
    [SerializeField]
    private GameObject missile;

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

        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale.x == 1 ? new Vector3(10, 10, 10) : new Vector3(1, 1, 1);

        float timeMoved = 0;
        while (timeMoved < carTranslationTime)
        {
            timeMoved += Time.deltaTime;
            transform.transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, timeMoved / carTranslationTime);
            transform.transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, timeMoved / carTranslationTime);
            transform.transform.localScale = Vector3.Lerp(startScale, endScale, timeMoved / carTranslationTime);
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

    public bool NeedsAmunition()
    {
        return missileHolder.childCount == 0;
    }

    public void UseAmunition()
    {
        missile = Instantiate(missilePrefab);
        missile.transform.parent = missileHolder;
        missile.transform.localPosition = Vector3.zero;
        missile.transform.localRotation = Quaternion.identity;
        missile.transform.localScale = new Vector3(1, 1, 1);
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

    // Upgrades

    public void ApplyHealthUpgrade()
    {
        health.AddArmor();
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
