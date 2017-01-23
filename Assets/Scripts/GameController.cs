using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private AudioSource themeSong;
    [SerializeField]
    private AudioSource startSound;

    [SerializeField]
    private Renderer screenRenderer;

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
    public Text itemText;
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
    private Transform upgradeCarPostion;
    [SerializeField]
    private Transform carStartPostion;

    [SerializeField]
    private Animator garageAnimator;
    [SerializeField]
    private AudioSource garageOpenSound;
    [SerializeField]
    private AudioSource garageCloseSound;

    [SerializeField]
    private AudioSource screenInterferenceSound;

    [SerializeField]
    private GameObject amunition;

    [SerializeField]
    private float empDuration = 5f;

    private int score;
    private bool restart;

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        upgradeController.enabled = false;
        playerController.EnableInteraction(false);

        scoreText.text = "";
        itemText.text = "";
        waveText.text = "";
        gameOverText.text = "";
        restartText.text = "";

        screenRenderer.sharedMaterial.SetColor("_EmissionColor", Color.black);
        DynamicGI.UpdateMaterials(screenRenderer);
    }

    public void StartGame()
    {
        startSound.Play();
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        float elapsedTime = 0.0f;
        float duration = 3.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            screenRenderer.sharedMaterial.SetColor("_EmissionColor", Color.white * 2 * t);
            DynamicGI.UpdateMaterials(screenRenderer);

            yield return null;
        }

        themeSong.Play();
        yield return new WaitForSeconds(3);

        int countdown = 5;
        while (countdown > 0)
        {
            scoreText.text = "" + countdown;
            countdown -= 1;
            yield return new WaitForSeconds(1);
        }
        scoreText.text = "";

        StartCoroutine(SpawnWave());
        playerController.EnableInteraction(true);
    }

    void Update()
    {
        if (restart && OVRInput.Get(OVRInput.Button.One))
            SceneManager.LoadScene("Map1");
    }

    private IEnumerator SpawnWave()
    {
        themeSong.volume = 0.5f;

        waveCount += 1;
        waveText.text = "Wave " + waveCount;
        garageAnimator.SetTrigger("Open");
        garageOpenSound.Play();

        yield return new WaitForSeconds(waveWait);

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
        garageCloseSound.Play();
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
        scoreText.text = "" + score;
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
        themeSong.volume = 0.5f;
        upgrades.SetActive(true);

        playerController.EnableInteraction(false);
        playerController.MoveTo(upgradeCarPostion);

        upgradeController.enabled = true;
    }

    public void UpgradeApplied()
    {
        upgradeController.enabled = false;
        upgrades.SetActive(false);

        playerController.MoveTo(carStartPostion);
        playerController.EnableInteraction(true);

        StartCoroutine(SpawnWave());
    }

    public void ShowAmunition()
    {
        amunition.SetActive(true);
    }

    public void EMP()
    {
        screenInterferenceSound.Play();
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().EnableInteraction(false);
        }

        StartCoroutine(EMPCoroutine());
    }

    private IEnumerator EMPCoroutine()
    {
        int counter = 0;
        float flickerDuration = 0.15f;
        int totalFlickers = Mathf.RoundToInt(empDuration / flickerDuration);

        while (counter < totalFlickers)
        {
            float elapsedTime = 0.0f;
            while (elapsedTime < flickerDuration)
            {
                float startIntensity = counter % 2 == 0 ? 0.2f : 0.3f;
                float endIntensity = counter % 2 == 0 ? 0.1f : -0.1f;

                elapsedTime += Time.deltaTime;
                float t = elapsedTime / flickerDuration;

                screenRenderer.sharedMaterial.SetColor("_EmissionColor", Color.white * (startIntensity + endIntensity * t));
                DynamicGI.UpdateMaterials(screenRenderer);

                yield return null;
            }

            counter += 1;
            yield return null;
        }
        Debug.Log("EMP Over");

        screenRenderer.sharedMaterial.SetColor("_EmissionColor", Color.white * 2);
        DynamicGI.UpdateMaterials(screenRenderer);

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
        themeSong.volume = 1;

        itemText.text = scoreText.text;
        scoreText.text = "";
        gameOverText.text = "Game Over!";
        yield return new WaitForSeconds(3);
        gameOverText.text = "";
        restartText.text = "Press 'A' to restart";
        restart = true;
    }
}
