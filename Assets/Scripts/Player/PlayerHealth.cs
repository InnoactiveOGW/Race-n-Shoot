using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : DestructableObject
{
    private GameController gameController;

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

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();

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

        if (currentHealth < startingHealth)
        {
            currentHealth = Mathf.Clamp(currentHealth + Time.deltaTime * regenSpeed, 0f, startingHealth);
        }

        SetHealthUI();
    }

    public void ResetHealth()
    {
        currentHealth = startingHealth;
        SetHealthUI();
    }

    public virtual void TakeDamage(float amount)
    {
        if (invincibilityTimer > 0f)
            return;

        base.TakeDamage(amount);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        gameController.PlayerDied();
        gameObject.SetActive(false);
    }

    public override void SetHealthUI()
    {
        base.SetHealthUI();

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
}
