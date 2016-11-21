using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
	[SerializeField] Transform player;
//	PlayerHealth playerHealth;
//	EnemyHealth enemyHealth;
	[SerializeField] NavMeshAgent navAgent;


	void Awake ()
	{
//		player = GameObject.FindGameObjectWithTag ("Player").transform;
//		playerHealth = player.GetComponent <PlayerHealth> ();
//		enemyHealth = GetComponent <EnemyHealth> ();
	}


	void Update ()
	{
//		if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0) {
			navAgent.SetDestination (player.position);
//		}
//		else {
//			nav.enabled = false;
//		}
	}
}
