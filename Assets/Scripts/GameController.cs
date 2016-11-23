using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GameObject enemy;
	public GameObject armouredEnemy;

	public Transform[] spawns;
	public float waveCount;

	public float startWait;
	public float spawnWait;
	public float waveWait;

	public Text scoreText;
	public Text restartText;
	public Text gameOverText;

	private int score;
	private bool gameOver;
	private bool restart;

	void Start ()
	{
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		UpdateScore ();
	}

	void Update ()
	{
		if (gameOver) {
			restartText.text = "Press 'R' for Restart";
			restart = true;
			break;
		}

		if (restart) {
			if (Input.GetKeyDown (KeyCode.R)) {
				SceneManager.LoadScene ("Map1");
			}
		}

		StartCoroutine (SpawnWave ());
	}

	IEnumerator SpawnWave ()
	{
		yield return new WaitForSeconds (waveWait);

		waveCount += 1;
		for (int i = 0; i < waveCount; i++) {
			Transform spawn = spawns [i % 3];

			if (i % 3 == 0) {
				Instantiate (armouredEnemy, spawn.position, spawn.rotation);
			} else {
				Instantiate (enemy, spawn.position, spawn.rotation);
			}
			yield return null;
		}
	}

	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}

	public void GameOver ()
	{
		gameOverText.text = "Game Over!";
		gameOver = true;
	}
}
