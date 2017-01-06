using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Transform mainCamerPostionNoVR;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject armouredEnemy;

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
    [SerializeField]
    private float carTranslationTime;

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

        for (int i = 0; i < waveCount; i++)
        {
            Transform spawn = spawns[i % 3];

            if (i > 0 && i % 3 == 0)
            {
                Instantiate(armouredEnemy, spawn.position, spawn.rotation);
            }
            else
            {
                Instantiate(enemy, spawn.position, spawn.rotation);
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
        if (waveCount % 1 == 0)
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

        player.GetComponent<PlayerController>().EnableInteraction(false);

        player.transform.parent = upgradeCarPostion;
        StartCoroutine(MoveCar());

        upgradeController.enabled = true;
    }

    public void UpgradeApplied()
    {
        upgradeController.enabled = false;
        upgrades.SetActive(false);

        Camera.main.transform.parent = mainCamerPostionNoVR;
        Camera.main.transform.transform.localRotation = Quaternion.identity;
        Camera.main.transform.transform.localPosition = Vector3.zero;

        player.transform.parent = null;
        StartCoroutine(MoveCar());

        player.GetComponent<PlayerController>().EnableInteraction(true);

        StartCoroutine(SpawnWave());
    }

    private IEnumerator MoveCar()
    {
        Vector3 startPosition = player.transform.localPosition;
        Quaternion startRotation = player.transform.localRotation;

        float timeMoved = 0;
        while (timeMoved < carTranslationTime)
        {
            timeMoved += Time.deltaTime;
            player.transform.transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, timeMoved / carTranslationTime);
            player.transform.transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, timeMoved / carTranslationTime);
            yield return null;
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
