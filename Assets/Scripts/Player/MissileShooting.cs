using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissileShooting : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private GameObject flying;
    [SerializeField]
    private GameObject fire;
    [SerializeField]
    private GameObject fireBall;
    [SerializeField]
    private Transform[] missileCheckpoints;
    [SerializeField]
    private AudioSource flyingSound;
    [SerializeField]
    private AudioSource explosionSound;
    [SerializeField]
    private float speed = 20f;
    [SerializeField]
    private float damage = 100f;

    private GameObject closestEnemy;
    private bool wasFired;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float trigger = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        if (trigger > 0f)
            Fire();
    }

    private void Fire()
    {
        if (wasFired)
            return;

        List<GameObject> enemyTargets = new List<GameObject>(GameObject.FindGameObjectsWithTag("EnemyTarget"));
        for (int i = 0; i < enemyTargets.Count; i++)
        {
            if (enemyTargets[i].GetComponentInParent<Health>().isDead)
            {
                enemyTargets.RemoveAt(i);
            }
        }

        Vector3[] enemyVectors = GetEnemyVectors(enemyTargets);
        if (enemyVectors.Length == 0)
            return;

        wasFired = true;

        gameObject.transform.parent = null;
        closestEnemy = enemyTargets[GetClosestEnemy(enemyVectors)];

        List<Vector3> positions = new List<Vector3>();
        positions.Add(transform.position);

        List<Transform> checkpoints = new List<Transform>(missileCheckpoints);
        while (positions.Count < 3 && checkpoints.Count > 0)
        {
            int index = Random.Range(0, checkpoints.Count - 1);
            Transform randomCheckpoint = checkpoints[index];
            checkpoints.Remove(randomCheckpoint);
            positions.Add(randomCheckpoint.position);
        }

        positions.Add(closestEnemy.transform.position);

        Vector3[] positionsArray = positions.ToArray();

        float distance = 0f;
        for (int i = 0; i < positionsArray.Length - 1; i++)
        {
            distance += Vector3.Distance(positionsArray[i], positionsArray[i + 1]);
        }

        LTDescr tweenId = LeanTween.move(this.gameObject, positionsArray, distance / speed).setEase(LeanTweenType.easeInQuad).setOrientToPath(true);
        tweenId.setOnComplete(tweenCompleted);

        flying.SetActive(true);
        flyingSound.Play();
    }

    private Vector3[] GetEnemyVectors(List<GameObject> enemyTargets)
    {
        Vector3[] enemyVectors = new Vector3[enemyTargets.Count];
        for (int i = 0; i < enemyTargets.Count; i++)
        {
            enemyVectors[i] = enemyTargets[i].transform.position - transform.position;
        }

        return enemyVectors;
    }

    // Find enemy closest to aim vector on the horizontal plane and return verticle angle to enemy.
    private int GetClosestEnemy(Vector3[] enemyVectors)
    {
        Vector3 aimVector = transform.forward;

        int closestEnemyIndex = 0;
        float minHorizontalAngle = float.MaxValue;
        for (int i = 0; i < enemyVectors.Length; i++)
        {
            Vector3 targDir = enemyVectors[i];
            targDir.y = 0;
            Vector3 cross = Vector3.Cross(targDir.normalized, transform.forward);
            float angleOnY = Mathf.Asin(cross.y) * Mathf.Rad2Deg;

            if (Mathf.Abs(angleOnY) < Mathf.Abs(minHorizontalAngle))
            {
                minHorizontalAngle = angleOnY;
                closestEnemyIndex = i;
            }
        }

        return closestEnemyIndex;
    }

    private void tweenCompleted()
    {
        if (!enabled)
            return;

        StartCoroutine(Rotate(Quaternion.LookRotation(closestEnemy.transform.position - transform.position), 0.1f));

        float distance = Vector3.Distance(transform.position, closestEnemy.transform.position);
        LTDescr tweenId = LeanTween.move(this.gameObject, closestEnemy.transform.position, distance / speed);
        tweenId.setOnComplete(tweenCompleted);
    }

    private IEnumerator Rotate(Quaternion rotation, float duration)
    {
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.rotation = Quaternion.Slerp(startRotation, rotation, t);
            yield return null;
        }
    }

    private IEnumerator FadeOutFireBall()
    {
        yield return new WaitForSeconds(0.5f);

        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0.0f;
        float duration = 0.8f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            Renderer renderer = fireBall.GetComponent<SkinnedMeshRenderer>();
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                Color color = renderer.materials[i].color;
                color.a = Mathf.Lerp(1f, 0f, t);
                renderer.materials[i].color = color;
            }
            yield return null;
        }

        Destroy(fireBall);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!wasFired)
            return;

        if (other.gameObject.tag == "Player")
            return;

        flyingSound.Stop();

        Health health = other.GetComponent<Health>();
        if (health != null)
            health.TakeDamage(damage);

        LeanTween.cancelAll();
        transform.rotation = Quaternion.identity;

        animator.SetTrigger("Explode");
        explosionSound.Play();

        StartCoroutine(FadeOutFireBall());
        fire.SetActive(true);

        Destroy(gameObject, 5);
    }


}
