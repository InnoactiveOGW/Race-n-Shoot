using UnityEngine;
using System.Collections;

public class PlayerGunRotation : MonoBehaviour
{
    [SerializeField]
    private Transform gunBarrelEnd;

    void Update()
    {
        float x = Input.GetAxisRaw("RightStickX");
        float z = Input.GetAxisRaw("RightStickY");

        if (x == 0 && z == 0)
            return;

        Vector3 stick = new Vector3(x, 0f, z).normalized;

        Vector3[] enemyVectors = getEnemyVectors();
        float verticalAngle = getVerticalAngleToClosestEnemy(enemyVectors);

        Vector3 dir = Vector3.zero - new Vector3(stick.x, verticalAngle, stick.z);
        // dir = new Vector3(dir.z, dir.x, dir.y);
        Quaternion newRotation = Quaternion.LookRotation(dir);
        Debug.Log("newRotation: " + newRotation.eulerAngles);
        transform.rotation = newRotation;
    }

    // Create vectors from gun barrel end to enemies.
    private Vector3[] getEnemyVectors()
    {
        GameObject[] enemyTargets = GameObject.FindGameObjectsWithTag("EnemyTarget");
        Vector3[] enemyVectors = new Vector3[enemyTargets.Length];
        for (int i = 0; i < enemyTargets.Length; i++)
        {
            enemyVectors[i] = enemyTargets[i].transform.position - gunBarrelEnd.position;
        }

        return enemyVectors;
    }

    // Find enemy closest to aim vector on the horizontal plane and return verticle angle to enemy.
    private float getVerticalAngleToClosestEnemy(Vector3[] enemyVectors)
    {
        if (enemyVectors.Length == 0)
            return 0f;

        Vector3 aimVector = gunBarrelEnd.forward;

        int closestEnemyIndex = 0;
        float minHorizontalAngle = float.MaxValue;
        for (int i = 0; i < enemyVectors.Length; i++)
        {
            Vector3 targDir = enemyVectors[i];
            targDir.y = 0;
            Vector3 cross = Vector3.Cross(targDir.normalized, gunBarrelEnd.forward);
            float angleOnY = Mathf.Asin(cross.y) * Mathf.Rad2Deg;

            if (Mathf.Abs(angleOnY) < Mathf.Abs(minHorizontalAngle))
            {
                minHorizontalAngle = angleOnY;
                closestEnemyIndex = i;
            }
        }

        if (Mathf.Abs(minHorizontalAngle) > 30f)
            return 0f;

        float verticalAngle = -enemyVectors[closestEnemyIndex].normalized.y;
        return Mathf.Clamp(verticalAngle, -0.2f, 0.2f);
    }
}
