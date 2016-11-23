using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
	[SerializeField] float regenSpeed = 1f;

	[SerializeField] RawImage bloodPulse;
	[SerializeField] GameObject bloodSplatter1;
	[SerializeField] GameObject bloodSplatter2;
	[SerializeField] GameObject bloodSplatter3;

	float previousHealth;
	float pulseSpeed = 1.5f;

	Color splatterZero;
	Color splatterLow;
	Color splatterFull;

	void Awake ()
	{
		splatterZero = bloodPulse.color;
		splatterLow = bloodPulse.color;
		splatterLow.a = 90f / 255f;
		splatterFull = bloodPulse.color;
		splatterFull.a = 180f / 255f;
	}

	void Update ()
	{
		if (currentHealth < startingHealth) {
			Debug.Log ("Health regen: " + currentHealth);
			currentHealth = Mathf.Clamp (currentHealth + Time.deltaTime * regenSpeed, 0f, startingHealth);
		}
			
		SetHealthUI ();
	}

	public override void OnDeath ()
	{
		base.OnDeath ();
//		gameObject.SetActive (false);
	}

	public override void SetHealthUI ()
	{
		bloodSplatter1.SetActive (currentHealth / startingHealth <= 0.75f);
		bloodSplatter2.SetActive (currentHealth / startingHealth <= 0.50f);
		bloodSplatter3.SetActive (currentHealth / startingHealth <= 0.25f);

		if (currentHealth / startingHealth <= 0.75f) {
			bloodPulse.color = Color.Lerp (splatterLow, splatterFull, Mathf.PingPong (Time.time * pulseSpeed, 1f));
		} else {
			bloodPulse.color = splatterZero;
		}
	}
}
