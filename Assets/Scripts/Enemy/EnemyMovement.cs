using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    private EnemyController enemyController;
    private NavMeshAgent navAgent;
    private Transform player;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;

    void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        navAgent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        player = GameObject.FindWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            navAgent.SetDestination(player.position);
        }
        else
        {
            navAgent.enabled = false;
            enemyController.StopEngine();
        }
    }
}
