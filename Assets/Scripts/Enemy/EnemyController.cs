using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private EnemyMovement movement;
    private EnemyGunRotation gunRotation;
    private EnemyShooting shooting;
    [SerializeField]
    private AudioSource stunnedSound;
    private AudioSource playingSound;
    [SerializeField]
    private AudioSource engineSound;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        movement = GetComponent<EnemyMovement>();
        gunRotation = GetComponentInChildren<EnemyGunRotation>();
        shooting = GetComponentInChildren<EnemyShooting>();
    }

    public void EnableInteraction(bool enabled)
    {
        if (enabled)
        {
            stunnedSound.Stop();
            engineSound.Play();
        }
        else
        {
            StopEngine();
            stunnedSound.Play();
        }

        navAgent.enabled = enabled;
        movement.enabled = enabled;
        gunRotation.enabled = enabled;
        shooting.enabled = enabled;
    }

    public void StopEngine()
    {
        engineSound.Stop();
    }
}
