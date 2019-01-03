using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;

public class GameController : MonoBehaviour {

    [SerializeField] private BallCore leftBall;
    [SerializeField] private BallCore rightBall;

    [SerializeField] private WaveController waveController;
    [SerializeField] private StageConfig[] stageConfigs;

    [SerializeField] private KeyCode leftBallHit;
    [SerializeField] private KeyCode rightBalHit;

    [SerializeField] private TextMeshProUGUI scoreText;

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
    private int leftPreviousScore;
    private int rightPreviousScore;

    private int leftScore = 0;
    private int rightScore = 0;
    private int score = 0;

    private int highScore = 0;

    private int currentStageIndex;
    private int stagesCount;

    private string savePath;

    private void Awake () {
        savePath = Path.Combine(Application.persistentDataPath, "saveFile");
        Load();
        UpdateHighScoreText();

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
            leftPreviousScore = leftScore;
        }

        if (Input.GetKeyDown(rightBalHit) && !IsRightBallHitPressed)
        {
            IsRightBallHitPressed = true;
            rightPreviousScore = rightScore;
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
                leftPreviousScore = leftScore;
            }

            if (touch.position.x > Screen.width / 2 && touch.phase == TouchPhase.Began && !IsRightBallHitPressed)
            {
                IsRightBallHitPressed = true;
                rightPreviousScore = rightScore;
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
                if (leftScore == leftPreviousScore)
                {
                    leftScore = leftScore <= 0 ? 0 : leftScore-1;
                    UpdateScore();
                }
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
                if (rightScore == rightPreviousScore)
                {
                    rightScore = rightScore <= 0 ? 0 : rightScore-1;
                    UpdateScore();
                }
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

        leftScore = rightScore = 0;
        UpdateScore();

        leftBall.ResetCrack();
        rightBall.ResetCrack();

        gameInProgress = true;
        waveController.IsRunning = true;

        highScoreText.enabled = false;
        startText.enabled = false;
    }

    public void StopGame()
    {
        if (gameInProgress)
        {
            gameInProgress = false;
            waveController.StopRunning();

            if (score > highScore)
            {
                highScore = score;
                Save();
            }

            timeSinceDeath = 0.0f;

            UpdateHighScoreText();
            highScoreText.enabled = true;

            startText.enabled = true;
        }
    }

    private void UpdateHighScoreText()
    {
        highScoreText.SetText("High Score: " + highScore.ToString());
    }

    public void WaveBlocked(bool isLeft)
    {
        if (isLeft)
        {
            leftScore++;
        } else
        {
            rightScore++;
        }

        UpdateScore();

        if(currentStageIndex + 1 < stagesCount)
        {
            if(score >= stageConfigs[currentStageIndex + 1].scoreToActivate)
            {
                currentStageIndex++;
                waveController.SetConfig(stageConfigs[currentStageIndex]);
            }
        }
    }

    private void UpdateScore()
    {
        score = leftScore + rightScore;
        scoreText.SetText(score.ToString());
    }

    private void Save()
    {
        using (
            var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
        ) {
            writer.Write(highScore);
        }
    }

    private void Load()
    {
        if (File.Exists(savePath)) {
            using (
                var reader = new BinaryReader(File.Open(savePath, FileMode.Open))
            ) {
                highScore = reader.ReadInt32();
            }
        }
    }
}
