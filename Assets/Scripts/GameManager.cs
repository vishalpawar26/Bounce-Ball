using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int lives = 3;
    private int score = 0;

    [SerializeField] GameObject[] players;
    [SerializeField] GameObject[] starCases;
    [SerializeField] GameObject pop;
    [SerializeField] GameObject graphics;
    [SerializeField] Transform checkPoint;
    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;
    [SerializeField] Text finalScoreText;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject levelCompletePanel;

    public static GameManager Instance { get; private set; }
    public GameObject player;

    private void Awake() => Instance = this;

    private void Start()
    {
        pop.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        levelCompletePanel.SetActive(false);

        InitialPlayer();
    }

    private void Update()
    {
        livesText.text = "X" + lives.ToString();
    }
    
    void InitialPlayer()
    {
        player = players[0];
        players[1].SetActive(false);
    }

    public void CollideWithObstacles()
    {
        lives--;
        player.SetActive(false);
        graphics.SetActive(false);
        pop.SetActive(true);

        StartCoroutine(SpawnPlayerAtCheckpoint());

        if(lives <= 0)
        {
            //isGameOver = true;
            gameOverPanel.SetActive(true);
            player.SetActive(false);
        }
    }

    IEnumerator SpawnPlayerAtCheckpoint()
    {
        if(lives > 0)
        {
            yield return new WaitForSeconds(0.5f);
            player.transform.position = checkPoint.position;
            player.SetActive(true);
            graphics.SetActive(true);
            pop.SetActive(false);
        }
    }

    public void Score()
    {
        score += 100;
        scoreText.text = score.ToString();
    }

    public void PauseGame()
    {
        player.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        player.SetActive(true);
    }

    public void OpenScene(int sceneIndex) => SceneManager.LoadScene(sceneIndex);

    public void ChangeBall(int currentBallIndex, int nextBallIndex)
    {
        players[currentBallIndex].SetActive(false);
        players[nextBallIndex].SetActive(true);
        players[nextBallIndex].transform.position = players[currentBallIndex].transform.position;
        player = players[nextBallIndex];
    }

    public void LevelCompleted()
    {
        levelCompletePanel.SetActive(true);

        finalScoreText.text = score.ToString();

        // 1 Star
        if (score <= 300 || ((score > 300 && score <= 600) && lives == 1))
        {
            starCases[0].SetActive(false);
        }

        // 2 Stars
        else if ((score > 300 && score <= 600) || (score > 600 && lives == 2))
        {
            starCases[0].SetActive(false);
            starCases[1].SetActive(false);
        }

        // 3 Stars
        else
        {
            starCases[0].SetActive(false);
            starCases[1].SetActive(false);
            starCases[2].SetActive(false);
        }
    }
}
