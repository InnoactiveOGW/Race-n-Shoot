using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navAgent;
    [SerializeField]
    private EnemyMovement movement;
    [SerializeField]
    private EnemyGunRotation gunRotation;
    [SerializeField]
    private EnemyShooting shooting;

    public void EnableInteraction(bool enabled)
    {
        Debug.Log("Enemy EnableInteraction: " + enabled);
        navAgent.enabled = enabled;
        movement.enabled = enabled;
        gunRotation.enabled = enabled;
        shooting.enabled = enabled;
    }
}
