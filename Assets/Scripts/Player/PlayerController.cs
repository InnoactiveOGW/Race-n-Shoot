using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private GameController gameController;

    private CharacterController characterController;
    private PlayerMovement movement;
    private PlayerHealth health;
    private PlayerGunRotation gunRotation;

    [SerializeField]
    private AudioSource engineSound;
    [SerializeField]
    private AudioSource boostSound;

    [SerializeField]
    private GameObject armour;

    [SerializeField]
    private GameObject doubleFireWeapon;
    [SerializeField]
    private GameObject gunBarrelEndSingle;
    [SerializeField]
    private GameObject gunBarrelEndDouble;

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
        gameController = FindObjectOfType<GameController>();

        characterController = GetComponent<CharacterController>();
        movement = GetComponent<PlayerMovement>();
        health = GetComponent<PlayerHealth>();
        gunRotation = GetComponentInChildren<PlayerGunRotation>();
    }

    void Start()
    {
        armour.SetActive(false);
        doubleFireWeapon.SetActive(false);
        gunRotation.gunBarrelEnd = gunBarrelEndSingle.transform;
    }

    public void EnableInteraction(bool enabled)
    {
        if (enabled)
            engineSound.Play();
        else
            engineSound.Stop();

        characterController.enabled = enabled;
        movement.enabled = enabled;
        health.enabled = enabled;
        gunRotation.enabled = enabled;
        gunBarrelEndSingle.SetActive(enabled);
        gunBarrelEndDouble.SetActive(enabled);
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
        Vector3 endScale = new Vector3(1, 1, 1);

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

    public void IsMoving()
    {
        engineSound.volume = 0.5f;
    }

    public void StoppedMoving()
    {
        engineSound.volume = 0.2f;
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
        engineSound.Stop();

        boostSound.time = 9.828f;
        boostSound.Play();
    }

    public void SpeedBoostStopped()
    {
        boostSound.Stop();
        engineSound.Play();
    }

    // Upgrades

    public void ApplyHealthUpgrade()
    {
        health.AddArmor();
    }

    public void ApplyDoubleFireUpgrade()
    {
        gunRotation.gunBarrelEnd = gunBarrelEndDouble.transform;
    }

    public void Death()
    {
        EnableInteraction(false);
        gameController.PlayerDied();
    }
}
