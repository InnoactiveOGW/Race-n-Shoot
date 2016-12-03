using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour
{
    public int damagePerShot = 5;
    public float timeBetweenBullets = 0.166f;
    public float bulletsPerBurst = 3;
    public float timeBetweenBursts = 0.5f;

    public float range = 20;

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
        bulletTimer += Time.deltaTime;
        burstTimer += Time.deltaTime;

        if (burstTimer >= timeBetweenBursts)
        {
            burstTimer = 0f;
        }
        else
        {

        }

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
