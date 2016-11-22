using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
	[SerializeField] RawImage bloodSplatter;

	Color splatterFull;
	Color splatterZero;

	void Awake ()
	{
		splatterFull = bloodSplatter.color;
		splatterZero = bloodSplatter.color;
		splatterZero.a = 0;
	}

	public override void OnDeath ()
	{
		base.OnDeath ();
		gameObject.SetActive (false);
	}

	public override void SetHealthUI ()
	{
		bloodSplatter.color = Color.Lerp (splatterZero, splatterFull, 1 - currentHealth / startingHealth);
	}
}
