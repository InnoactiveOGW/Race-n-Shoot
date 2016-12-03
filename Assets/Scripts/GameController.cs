using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameObject enemy;
    public GameObject armouredEnemy;

    public Transform[] spawns;
    float waveCount;
    float enemyCount;

    public float waveWait;
    public float spawnWait;

    public Text scoreText;
    public Text waveText;
    public Text gameOverText;
    public Text restartText;

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

    IEnumerator SpawnWave()
    {
        waveCount += 1;
        waveText.text = "Wave " + waveCount;
        yield return new WaitForSeconds(waveWait);
        waveText.text = "";

        for (int i = 0; i < waveCount; i++)
        {
            Transform spawn = spawns[i % 3];

            if (i % 3 == 0)
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

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    IEnumerator GameOver()
    {
        gameOverText.text = "Game Over!";
        yield return new WaitForSeconds(3);
        gameOverText.text = "";
        restartText.text = "Press 'R' for Restart";
        restart = true;
    }

    public void EnemyKilled(int value)
    {
        score += value;
        UpdateScore();

        enemyCount -= 1;
        if (enemyCount == 0)
        {
            StartCoroutine(SpawnWave());
        }
    }

    public void PlayerDied()
    {
        StartCoroutine(GameOver());
    }
}
