using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : FireAnimationObject
{
    [SerializeField]
    private PlayerController playerController;

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
    [SerializeField]
    private GameObject invincibilityShield;

    private bool hasArmor = false;
    public float startingArmor;
    public float currentArmor;

    [SerializeField]
    private GameObject frontArmor;
    [SerializeField]
    private GameObject sideArmor;
    [SerializeField]
    private GameObject backArmor;

    private const float frontArmorBoundary = 75f;
    private const float sideArmorBoundary = 37.5f;

    void Awake()
    {
        splatterZero = bloodPulse.color;
        splatterLow = bloodPulse.color;
        splatterLow.a = 90f / 255f;
        splatterFull = bloodPulse.color;
        splatterFull.a = 180f / 255f;

        startingArmor = 0f;
        currentArmor = 0f;
    }

    void Update()
    {
        if (invincibilityTimer > 0f)
        {
            invincibilityShield.SetActive(true);
            invincibilityTimer = Mathf.Max(0f, invincibilityTimer - Time.deltaTime);
            if (invincibilityTimer == 0f)
                invincibilityShield.SetActive(false);
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

    public bool NeedsHealthpack()
    {
        return currentArmor < startingArmor || currentHealth < startingHealth;
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
        else
        {
            backArmor.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0);
            sideArmor.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0);
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();
        playerController.Death();
    }

}
