﻿using UnityEngine;
using UnityEngine.UI;

public abstract class Health : MonoBehaviour
{
	public float startingHealth = 100f;

	public float currentHealth;
	bool isDead;

	void OnEnable ()
	{
		currentHealth = startingHealth;
		isDead = false;
		SetHealthUI ();
	}


	public virtual void TakeDamage (float amount)
	{
		currentHealth -= amount;
		SetHealthUI ();

		if (currentHealth <= 0f && !isDead) {
			OnDeath ();
		}
	}

	public virtual void OnDeath ()
	{
		isDead = true;
	}

	public abstract void SetHealthUI ();
}
