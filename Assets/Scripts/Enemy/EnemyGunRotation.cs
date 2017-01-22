using UnityEngine;
using System.Collections;

public class EnemyGunRotation : MonoBehaviour
{
    public float turnSpeed = 0.1f;
    private Transform player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        Quaternion previous = transform.rotation;

        Vector3 dir = player.position - transform.position;
        Quaternion newRotation = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Lerp(previous, newRotation, Time.time * turnSpeed);
    }
}
