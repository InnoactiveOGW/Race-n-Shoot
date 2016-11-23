using UnityEngine;
using System.Collections;

public class EnemyHealth : Health
{
	[SerializeField] Renderer _renderer;
	[SerializeField] Color fullHealthColor;
	[SerializeField] Color zeroHealthColor;

	public override void OnDeath ()
	{
		base.OnDeath ();
//		gameObject.SetActive (false);
	}

	public override void SetHealthUI ()
	{
		_renderer.material.color = Color.Lerp(fullHealthColor, zeroHealthColor, 1 - currentHealth / startingHealth);
	}
}
