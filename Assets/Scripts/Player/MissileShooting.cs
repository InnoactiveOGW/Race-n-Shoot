using UnityEngine;
using System.Collections;

public class MissileShooting : MonoBehaviour
{
    [SerializeField]
    private Transform[] missileCheckpoints;

    void Update()
    {
        if (!Input.GetButton("Fire2"))
            return;

        Debug.Log("fire missile");

        GameObject[] enemyTargets = GameObject.FindGameObjectsWithTag("EnemyTarget");
        Vector3[] enemyVectors = getEnemyVectors(enemyTargets);
        if (enemyVectors.Length == 0)
            return;

        GameObject closestEnemy = enemyTargets[getClosestEnemy(enemyVectors)];

        LeanTween.move(this.gameObject, [transform.position, closestEnemy.transform.position], 1.0);
    }

    private Vector3[] getEnemyVectors(GameObject[] enemyTargets)
    {
        Vector3[] enemyVectors = new Vector3[enemyTargets.Length];
        for (int i = 0; i < enemyTargets.Length; i++)
        {
            enemyVectors[i] = enemyTargets[i].transform.position - transform.position;
        }

        return enemyVectors;
    }

    // Find enemy closest to aim vector on the horizontal plane and return verticle angle to enemy.
    private int getClosestEnemy(Vector3[] enemyVectors)
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

        return closestEnemyIndex
    }
}
