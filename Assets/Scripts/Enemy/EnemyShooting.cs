using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour
{
	[SerializeField] GameObject player;

	public int damagePerShot = 10;
	public float timeBetweenBullets = 0.15f;

	float timer;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	[SerializeField] LineRenderer gunLine;
	[SerializeField] Light gunLight;
	[SerializeField] AudioSource gunAudio;

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

		// Try and find an EnemyHealth script on the gameobject hit.

		// If the EnemyHealth component exist...
		if (false) {
			// ... the enemy should take damage.
		}

		gunLine.SetPosition (1, shootHit.point);
	}
}
