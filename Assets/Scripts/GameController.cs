using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Transform mainCamerPostionNoVR;

    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject armouredEnemy;

    private List<GameObject> enemies;

    [SerializeField]
    private Transform[] spawns;
    private float waveCount;
    private float enemyCount;

    [SerializeField]
    private float waveWait;
    [SerializeField]
    private float spawnWait;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text waveText;
    [SerializeField]
    private Text gameOverText;
    [SerializeField]
    private Text restartText;

    [SerializeField]
    private UpgradeController upgradeController;
    [SerializeField]
    private GameObject upgrades;
    [SerializeField]
    private Transform upgradeCamerPostionNoVR;
    [SerializeField]
    private Transform upgradeCarPostion;

    private int score;
    private bool restart;

    void Awake()
    {
        waveText.text = "";
        gameOverText.text = "";
        restartText.text = "";
        UpdateScore();
    }

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Map1");
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        waveCount += 1;
        waveText.text = "Wave " + waveCount;
        yield return new WaitForSeconds(waveWait);
        waveText.text = "";

        if (waveCount > 1)
            foreach (GameObject enemy in enemies)
                Destroy(enemy);
        enemies = new List<GameObject>();

        for (int i = 0; i < waveCount; i++)
        {
            Transform spawn = spawns[i % 3];

            if (i > 0 && i % 3 == 0)
            {
                enemies.Add((GameObject)Instantiate(armouredEnemy, spawn.position, spawn.rotation));
            }
            else
            {
                enemies.Add((GameObject)Instantiate(enemy, spawn.position, spawn.rotation));
            }

            enemyCount += 1;

            yield return new WaitForSeconds(spawnWait);
        }

    }

    public void EnemyKilled(int value)
    {
        score += value;
        UpdateScore();

        enemyCount -= 1;
        if (enemyCount == 0)
        {
            WaveFinished();
        }
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    private void WaveFinished()
    {
        playerController.ResetHealth();

        if (!upgradeController.hasAllUpgrades && waveCount % 1 == 0)
        {
            ShowUpgrades();
        }
        else
        {
            StartCoroutine(SpawnWave());
        }
    }

    private void ShowUpgrades()
    {
        upgrades.SetActive(true);

        Camera.main.transform.parent = upgradeCamerPostionNoVR;
        Camera.main.transform.transform.localRotation = Quaternion.identity;
        Camera.main.transform.transform.localPosition = Vector3.zero;

        playerController.EnableInteraction(false);
        playerController.MoveTo(upgradeCarPostion);

        upgradeController.enabled = true;
    }

    public void UpgradeApplied()
    {
        upgradeController.enabled = false;
        upgrades.SetActive(false);

        Camera.main.transform.parent = mainCamerPostionNoVR;
        Camera.main.transform.transform.localRotation = Quaternion.identity;
        Camera.main.transform.transform.localPosition = Vector3.zero;

        playerController.MoveTo(null);
        playerController.EnableInteraction(true);

        StartCoroutine(SpawnWave());
    }

    public void PlayerDied()
    {
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        gameOverText.text = "Game Over!";
        yield return new WaitForSeconds(3);
        gameOverText.text = "";
        restartText.text = "Press 'R' for Restart";
        restart = true;
    }
}
