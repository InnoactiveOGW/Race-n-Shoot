using UnityEngine;
using System.Collections;

public class MachineGunShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    private float effectsDisplayTime = 0.2f;

    private LineRenderer gunLine;
    private Light gunLight;
    [SerializeField]
    private AudioSource[] gunSounds;
    [SerializeField]
    private AudioSource[] shellSounds;

    private int shootableMask;
    private float timer;
    private Ray shootRay;
    private RaycastHit shootHit;

    void Awake()
    {
        gunLine = GetComponent<LineRenderer>();
        gunLight = GetComponent<Light>();

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

        AudioSource gunSound = gunSounds[Random.Range(0, gunSounds.Length - 1)];
        gunSound.volume = 0.5f;
        gunSound.Play();

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

        StartCoroutine(PlayShellSound());
    }

    private IEnumerator PlayShellSound()
    {
        yield return new WaitForSeconds(0.5f);
        AudioSource shellSound = shellSounds[Random.Range(0, shellSounds.Length - 1)];
        shellSound.volume = 0.2f;
        shellSound.Play();
    }
}
