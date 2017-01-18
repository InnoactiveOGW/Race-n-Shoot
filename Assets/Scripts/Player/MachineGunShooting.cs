using UnityEngine;
using System.Collections;

public class MachineGunShooting : MonoBehaviour
{

    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;

    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    [SerializeField]
    LineRenderer gunLine;
    [SerializeField]
    Light gunLight;
    [SerializeField]
    AudioSource gunAudio;
    float effectsDisplayTime = 0.2f;

    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        Vector2 input = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        float x = input.x;
        float z = input.y;

        if ((x != 0 || z != 0) && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
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
        timer = 0f;

        gunAudio.Play();

        gunLight.enabled = true;

        gunLine.SetPosition(0, Vector3.zero);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            Health health = shootHit.collider.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damagePerShot);
            }

            gunLine.SetPosition(1, transform.InverseTransformPoint(shootHit.point));

        }
        else
        {
            gunLine.SetPosition(1, transform.InverseTransformPoint(shootRay.origin + shootRay.direction * range));
        }

        gunLine.enabled = true;
    }
}
