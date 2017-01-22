using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private EnemyMovement movement;
    private EnemyGunRotation gunRotation;
    private EnemyShooting shooting;
    [SerializeField]
    private AudioSource[] stunnedSounds;
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
            if (playingSound != null)
            {
                playingSound.Stop();
            }
        }
        else
        {
            int soundIndex = Random.Range(0, stunnedSounds.Length - 1);
            playingSound = stunnedSounds[soundIndex];
            playingSound.Play();
        }

        Debug.Log("Enemy EnableInteraction: " + enabled);
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
