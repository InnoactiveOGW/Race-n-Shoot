using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : DestructableObject
{
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private GameObject car;
    [SerializeField]
    private GameObject carExplosion;

    [SerializeField]
    private float regenSpeed = 1f;

    [SerializeField]
    private RawImage bloodPulse;
    [SerializeField]
    private GameObject bloodSplatter1;
    [SerializeField]
    private GameObject bloodSplatter2;
    [SerializeField]
    private GameObject bloodSplatter3;

    private float previousHealth;
    private float pulseSpeed = 1.5f;

    private Color splatterZero;
    private Color splatterLow;
    private Color splatterFull;

    [HideInInspector]
    public float invincibilityTimer;

    private bool hasArmor = false;
    private float startingArmor;
    private float currentArmor;

    [SerializeField]
    private GameObject frontArmor;
    [SerializeField]
    private GameObject sideArmor;
    [SerializeField]
    private GameObject backArmor;

    private const float frontArmorBoundary = 75f;
    private const float sideArmorBoundary = 37.5f;

    [SerializeField]
    private SkinnedMeshRenderer firstFire;
    [SerializeField]
    private SkinnedMeshRenderer seconedFire;

    void Awake()
    {
        splatterZero = bloodPulse.color;
        splatterLow = bloodPulse.color;
        splatterLow.a = 90f / 255f;
        splatterFull = bloodPulse.color;
        splatterFull.a = 180f / 255f;
    }

    void Update()
    {
        if (invincibilityTimer > 0f)
        {
            invincibilityTimer = Mathf.Max(0f, invincibilityTimer - Time.deltaTime);
        }

        if (!isDead && currentHealth < startingHealth)
        {
            currentHealth = Mathf.Clamp(currentHealth + Time.deltaTime * regenSpeed, 0f, startingHealth);
        }

        SetHealthUI();
    }

    public void ResetHealth()
    {
        currentHealth = startingHealth;
        currentArmor = startingArmor;
        SetHealthUI();
    }

    public void AddArmor()
    {
        hasArmor = true;
        startingArmor = 100;
        currentArmor = startingArmor;
    }

    public override void TakeDamage(float amount)
    {
        if (invincibilityTimer > 0f)
            return;

        if (currentArmor > 0)
        {
            currentArmor -= amount;
            if (currentArmor < 0)
            {
                amount = currentArmor;
                currentArmor = 0;
            }
            else
            {
                amount = 0;
            }
        }

        base.TakeDamage(amount);
    }

    public override void SetHealthUI()
    {
        base.SetHealthUI();

        if (hasArmor)
        {
            SetArmorUI();
        }

        bloodSplatter1.SetActive(currentHealth / startingHealth <= 0.75f);
        bloodSplatter2.SetActive(currentHealth / startingHealth <= 0.50f);
        bloodSplatter3.SetActive(currentHealth / startingHealth <= 0.25f);

        if (currentHealth / startingHealth <= 0.75f)
        {
            bloodPulse.color = Color.Lerp(splatterLow, splatterFull, Mathf.PingPong(Time.time * pulseSpeed, 1f));
        }
        else
        {
            bloodPulse.color = splatterZero;
        }
    }

    private void SetArmorUI()
    {
        if (currentArmor <= 0)
        {
            if (backArmor != null)
            {
                // backArmor.transform.parent = null;
                // backArmor = null;
            }
        }
        else if (currentArmor < sideArmorBoundary)
        {
            if (sideArmor != null)
            {
                // sideArmor.transform.parent = null;
                // sideArmor = null;
            }

            float backArmorHealth = (1 - (currentArmor / sideArmorBoundary)) * 100;
            backArmor.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, backArmorHealth);
        }
        else if (currentArmor < frontArmorBoundary)
        {
            if (frontArmor != null)
            {
                // frontArmor.transform.parent = null;
                // frontArmor = null;
            }

            float sideArmorHealth = (1 - ((currentArmor - sideArmorBoundary) / (frontArmorBoundary - sideArmorBoundary))) * 100;
            sideArmor.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, sideArmorHealth);
        }
    }

    public override void OnDeath()
    {
        carExplosion.SetActive(true);
        car.SetActive(false);

        carExplosion.GetComponent<Animation>().Play();
        StartCoroutine(FireFlicker(firstFire));
        StartCoroutine(FireFlicker(seconedFire));
        StartCoroutine(SetActiveAfter(seconedFire.gameObject, 1.6f));

        playerController.Death();
    }

    private IEnumerator SetActiveAfter(GameObject gameObject, float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(true);
    }

    private IEnumerator FireFlicker(SkinnedMeshRenderer fire)
    {
        while (true)
        {
            fire.SetBlendShapeWeight(0, Random.Range(0, 100));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
