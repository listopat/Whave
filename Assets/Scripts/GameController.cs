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

    private int currentStageIndex;
    private int stagesCount;

    private void Start () {
        stagesCount = stageConfigs.Length;
        gameInProgress = false;
    }
	
	private void Update () {
        #if UNITY_ANDROID
        CheckForQuit();
        #endif

        if (gameInProgress)
        {
            GameUpdate();
        }
        else
        {
            DeathScreenUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (gameInProgress)
        {
            GameFixedUpdate();
        }
    }

    private void GameUpdate()
    {
        #if UNITY_ANDROID
        MobileGameUpdate();
        #endif

        #if UNITY_EDITOR
        DesktopGameUpdate();
        #endif
    }

    private void DesktopGameUpdate()
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

    private void MobileGameUpdate()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.position.x < Screen.width / 2 && touch.phase == TouchPhase.Began && !IsLeftBallHitPressed)
            {
                IsLeftBallHitPressed = true;
            }

            if (touch.position.x > Screen.width / 2 && touch.phase == TouchPhase.Began && !IsRightBallHitPressed)
            {
                IsRightBallHitPressed = true;
            }
        }
    }

    private void DeathScreenUpdate()
    {
        #if UNITY_ANDROID
        MobileDeathScreenUpdate();
        #endif

        #if UNITY_EDITOR
        DesktopDeathScreenUpdate();
        #endif
    }

    private void MobileDeathScreenUpdate()
    {
        if (timeSinceDeath > startAfterDeathDelay)
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    NewGame();
                }
            }
        }
        else
        {
            timeSinceDeath += Time.deltaTime;
        }
    }

    private void DesktopDeathScreenUpdate()
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

    private void CheckForQuit()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack", true);
        }
    }

    private void NewGame()
    {
        waveController.SetConfig(stageConfigs[0]);
        currentStageIndex = 0;

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
        waveController.StopRunning();
        
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

        if(currentStageIndex + 1 < stagesCount)
        {
            if(score >= stageConfigs[currentStageIndex + 1].scoreToActivate)
            {
                currentStageIndex++;
                waveController.SetConfig(stageConfigs[currentStageIndex]);
            }
        }
    }
}
