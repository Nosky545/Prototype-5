using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;

    private float spawnRate = 1f;

    public TextMeshProUGUI livesText;
    private int lives = 3;

    public TextMeshProUGUI scoreText;
    private int score;

    public TextMeshProUGUI gameOverText;

    public bool isGameActive;

    public Button restartButton;

    public GameObject titleScreen;

    private AudioSource music;
    private Slider volumeSlider;

    public GameObject pauseScreen;
    public bool isGamePaused = false;

    void Start()
    {
        music = GameObject.Find("Music").GetComponent<AudioSource>();
        volumeSlider = GameObject.Find("Volume Slider").GetComponent<Slider>();
    }
    
    void Update()
    {
        music.volume = volumeSlider.value;

        if (lives == 0)
        {
            GameOver();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
        {
            if (isGamePaused)
            {
                ResumeGame();
            }

            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void UpdateLives()
    {
        lives--;
        livesText.text = "Lives: " + lives;
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;
        spawnRate /= difficulty;
        titleScreen.SetActive(false);
        scoreText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
        StartCoroutine(SpawnTarget());
        UpdateScore(0);
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }
}
