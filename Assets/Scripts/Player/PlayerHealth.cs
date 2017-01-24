using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : FireAnimationObject
{
    private PlayerController playerController;
    private Renderer[] renderers;

    [SerializeField]
    private AudioLowPassFilter lowPassFilter;

    [SerializeField]
    private float regenSpeed = 1f;

    [SerializeField]
    private Color fullHealthColor;
    [SerializeField]
    private Color zeroHealthColor;

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
        playerController = GetComponent<PlayerController>();
        List<Renderer> renderersList = new List<Renderer>(GetComponentsInChildren<MeshRenderer>());
        renderersList.AddRange(GetComponentsInChildren<SkinnedMeshRenderer>());
        renderers = renderersList.ToArray();



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

        float relativeHealth = currentHealth / startingHealth;
        if (relativeHealth < 0.4f)
        {
            lowPassFilter.enabled = true;
            lowPassFilter.cutoffFrequency = 300 + (4700 * (relativeHealth / 0.4f));
        }
        else
        {
            lowPassFilter.enabled = false;
        }

        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = Color.Lerp(fullHealthColor, zeroHealthColor, 1 - relativeHealth);
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
        lowPassFilter.enabled = false;
        base.OnDeath();
        playerController.Death();
    }

}
