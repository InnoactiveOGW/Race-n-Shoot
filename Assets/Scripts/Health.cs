using UnityEngine;
using UnityEngine.UI;

public abstract class Health : MonoBehaviour
{
    public float startingHealth = 100f;
    public float currentHealth;
    public bool isDead;

    [SerializeField]
    private AudioSource hitSound;
    private AudioSource[] hitSounds;
    [SerializeField]
    private AudioSource deathSound;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        hitSounds = GetComponentsInChildren<AudioSource>();
    }

    void OnEnable()
    {
        currentHealth = startingHealth;
        isDead = false;
        SetHealthUI();
    }

    public virtual void TakeDamage(float amount)
    {
        if (hitSound != null)
            hitSound.Play();

        if (hitSounds != null && hitSounds.Length > 0)
            hitSounds[Random.Range(0, hitSounds.Length - 1)].Play();

        currentHealth -= amount;
        SetHealthUI();

        if (currentHealth <= 0f && !isDead)
        {
            isDead = true;
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        if (deathSound != null)
        {
            Debug.Log("death animation");
            deathSound.Play();
        }
        else
        {
            Debug.Log("no death animation");
        }
    }

    public abstract void SetHealthUI();
}
