using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Light mainLight;

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
    [SerializeField]
    private float upgradeWave;
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

    [SerializeField]
    private Animator garageAnimator;

    [SerializeField]
    private GameObject amunition;

    [SerializeField]
    private float empDuration = 5f;

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

    public void StartGame()
    {
        mainLight.enabled = true;
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
        garageAnimator.SetTrigger("Open");
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

        yield return new WaitForSeconds(2);
        garageAnimator.SetTrigger("Close");
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

        if (!upgradeController.hasAllUpgrades && waveCount % upgradeWave == 0)
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

        playerController.EnableInteraction(false);
        playerController.MoveTo(upgradeCarPostion);

        upgradeController.enabled = true;
    }

    public void UpgradeApplied()
    {
        upgradeController.enabled = false;
        upgrades.SetActive(false);

        playerController.MoveTo(null);
        playerController.EnableInteraction(true);

        StartCoroutine(SpawnWave());
    }

    public void ShowAmunition()
    {
        amunition.SetActive(true);
    }

    public void EMP()
    {
        StartCoroutine(EMPCoroutine());
    }

    private IEnumerator EMPCoroutine()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().EnableInteraction(false);
        }

        yield return new WaitForSeconds(empDuration);

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().EnableInteraction(true);
        }
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
