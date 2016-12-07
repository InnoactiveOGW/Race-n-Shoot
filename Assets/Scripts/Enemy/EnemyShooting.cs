using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenBursts = 1f;
    [SerializeField]
    private float bulletsPerBurst = 3;
    [SerializeField]
    private float timeBetweenBullets = 0.166f;
    [SerializeField]
    private int damagePerShot = 5;

    float range = 20;

    [SerializeField]
    private LineRenderer gunLine;
    [SerializeField]
    private Light gunLight;
    [SerializeField]
    private AudioSource gunAudio;

    private GameObject player;
    private PlayerHealth playerHealth;

    private float burstTimer;
    private float bulletTimer;
    private float bulletCounter;

    private Ray shootRay;
    private RaycastHit shootHit;
    private int shootableMask;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (bulletCounter >= bulletsPerBurst)
        {
            bulletCounter = 0;
            burstTimer = 0f;

            DisableEffects();
            return;
        }

        burstTimer += Time.deltaTime;
        if (burstTimer < timeBetweenBursts)
        {
            DisableEffects();
            return;
        }

        bulletTimer += Time.deltaTime;

        shootRay = new Ray(transform.position, transform.forward);
        if (bulletTimer >= timeBetweenBullets && Physics.Raycast(shootRay, out shootHit, range) && shootHit.collider.gameObject == player)
        {
            Shoot();
        }
        else
        {
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    void Shoot()
    {
        bulletTimer = 0f;
        bulletCounter += 1;

        gunAudio.Play();

        gunLight.enabled = true;

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);


        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damagePerShot);
        }

        gunLine.SetPosition(1, shootHit.point);
    }
}
