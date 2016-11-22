using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour
{
	public int damagePerShot = 10;
	public float timeBetweenBullets = 0.15f;

	GameObject player;
	PlayerHealth playerHealth;
	
	float timer;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	[SerializeField] LineRenderer gunLine;
	[SerializeField] Light gunLight;
	[SerializeField] AudioSource gunAudio;

	void Awake ()
	{
		player = GameObject.FindWithTag ("Player");
		playerHealth = player.GetComponent<PlayerHealth> ();
	}

	void Update ()
	{
		timer += Time.deltaTime;
		
		shootRay = new Ray (transform.position, transform.forward);
		if (timer >= timeBetweenBullets && Physics.Raycast (shootRay, out shootHit) && shootHit.collider.gameObject == player) {
			Shoot ();
		} else {
			DisableEffects ();
		}
	}

	public void DisableEffects ()
	{
		gunLine.enabled = false;
		gunLight.enabled = false;
	}

	void Shoot ()
	{
		timer = 0f;

		gunAudio.Play ();

		gunLight.enabled = true;

		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);


		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;

		if (playerHealth != null) {
			playerHealth.TakeDamage (damagePerShot);
		}

		gunLine.SetPosition (1, shootHit.point);
	}
}
