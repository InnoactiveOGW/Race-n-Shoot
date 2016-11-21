using UnityEngine;
using System.Collections;

public class EnemyGunRotation : MonoBehaviour
{
	public float turnSpeed = 0.1F;
	Transform player;

	void Awake ()
	{
		player = GameObject.FindWithTag ("PlayerTarget").GetComponent<Transform> ();
	}

	void Update ()
	{
		Quaternion previous = transform.rotation;

		Vector3 dir = player.position - transform.position;
		Quaternion newRotation = Quaternion.LookRotation (dir);

		transform.rotation = Quaternion.Lerp (previous, newRotation, Time.time * turnSpeed);
	}
}
