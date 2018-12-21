using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField] private BallCore leftBall;
    [SerializeField] private BallCore rightBall;

    [SerializeField] private WaveController waveController;
    [SerializeField] private StageConfig[] stageConfigs;

    [SerializeField] private KeyCode leftBallHit;
    [SerializeField] private KeyCode rightBalHit;

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI startText;

    private bool gameInProgress = false;

    private float startAfterDeathDelay = 0.5f;
    private float timeSinceDeath = 0.0f;

    public bool IsLeftBallHitPressed { get; private set; }
    public bool IsRightBallHitPressed { get; private set; }
    private int gracePeriod = 1;
    private int leftGracePeriodCounter = 0;
    private int rightGracePeriodCounter = 0;

    private int score = 0;
    private int highScore = 0;

    void Start () {
        gameInProgress = false;
    }
	
	void Update () {
        if (gameInProgress)
        {
            GameUpdate();
        }
        else
        {
            DeathScreenUpdate();
        }
    }

    void FixedUpdate()
    {
        if (gameInProgress)
        {
            GameFixedUpdate();
        }
    }

    private void GameUpdate()
    {
        if (Input.GetKeyDown(leftBallHit) && !IsLeftBallHitPressed)
        {
            IsLeftBallHitPressed = true;
        }

        if (Input.GetKeyDown(rightBalHit) && !IsRightBallHitPressed)
        {
            IsRightBallHitPressed = true;
        }
    }

    private void DeathScreenUpdate()
    {
        if (Input.GetKeyDown(leftBallHit) && timeSinceDeath > startAfterDeathDelay)
        {
            NewGame();
        }
        else
        {
            timeSinceDeath += Time.deltaTime;
        }
    }

    private void GameFixedUpdate()
    {
        if (IsLeftBallHitPressed)
        {
            if (leftGracePeriodCounter == gracePeriod)
            {
                IsLeftBallHitPressed = false;
                leftGracePeriodCounter = 0;
            }
            else
            {
                leftGracePeriodCounter++;
            }
        }

        if (IsRightBallHitPressed)
        {
            if (rightGracePeriodCounter == gracePeriod)
            {
                IsRightBallHitPressed = false;
                rightGracePeriodCounter = 0;
            }
            else
            {
                rightGracePeriodCounter++;
            }
        }
    }

    public void NewGame()
    {
        waveController.SetConfig(stageConfigs[0]);

        score = 0;
        scoreText.SetText(score.ToString());

        gameInProgress = true;
        waveController.IsRunning = true;

        highScoreText.enabled = false;
        startText.enabled = false;
        deathScreen.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void StopGame()
    {
        gameInProgress = false;
        waveController.IsRunning = false;

        if (score > highScore)
        {
            highScore = score;
        }

        timeSinceDeath = 0.0f;
        deathScreen.GetComponent<SpriteRenderer>().enabled = true;

        highScoreText.SetText("High Score: " + highScore.ToString());
        highScoreText.enabled = true;

        startText.enabled = true;
    }

    public void WaveBlocked()
    {
        score += 1;
        scoreText.SetText(score.ToString());
    }
}
