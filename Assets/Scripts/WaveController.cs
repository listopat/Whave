using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {

    [SerializeField] private Wave wavePrefab;
    [SerializeField] private WaveFactory waveFactory;

    [SerializeField] private Transform leftBall;
    [SerializeField] private Transform rightBall;
    [SerializeField] Color[] waveColors;

    private Vector3 leftSpawn = new Vector3(-22, 0, 0);
    private Vector3 rightSpawn = new Vector3(22, 0, 0);

    private StageConfig currentConfig;
    private float timeToNextPattern = 0;

    private readonly float waveRadius = 5f;
    private readonly float ballRadius = 0.4f;

    public bool IsRunning { get; set; }

    private List<Wave> waves;

    private void Start()
    {
        IsRunning = false;
        waves = new List<Wave>();
    }

    private void Update()
    {
        if (IsRunning)
        {
            SpawnLoop();
        }
    }

    private void SpawnLoop()
    {
        timeToNextPattern -= Time.deltaTime;

        if (timeToNextPattern <= 0)
        {
            timeToNextPattern += currentConfig.patternDelay;
            CreatePattern();
        }
        else
        {
            timeToNextPattern -= Time.deltaTime;
        }
    }

    public void SetConfig(StageConfig config)
    {
        currentConfig = config;
    }

    
    private void CreatePattern()
    {
        
        Pattern pattern = currentConfig.patterns[Random.Range(0, currentConfig.patterns.Length)];
        Color patternColor = waveColors[Random.Range(0, waveColors.Length)];

        int offsetsSum = 0;
        float firstWaveTimeToReach = 0;

        for (int i = 0; i < pattern.waves.Length; i++)
        {
            Wave wave = waveFactory.getWave();
            if(i == 0)
            {
                ConfigureWave(wave, patternColor, false);
            } else
            {
                ConfigureWave(wave, patternColor, pattern.randomizableSpeed);
            }

            Transform ball = pattern.waves[i].side == Side.Left ? leftBall : rightBall;
            Vector3 spawnPosition = pattern.waves[i].side == Side.Left ? leftSpawn : rightSpawn;
            offsetsSum += pattern.waves[i].offset;
            spawnPosition = RotateSpawnPoint(spawnPosition, ball.position);
            

            if (i == 0)
            {
                firstWaveTimeToReach = ((ball.position - spawnPosition).magnitude - waveRadius - ballRadius) / wave.moveSpeed;
            }

            PositionWave(wave, ball, spawnPosition, offsetsSum, firstWaveTimeToReach);

            waves.Add(wave);
        }
        
    }

    private Vector3 RotateSpawnPoint(Vector3 spawnPoint, Vector3 pivot)
    {
       return Quaternion.Euler(0.0f, 0.0f, Random.Range(-currentConfig.spawnAngle, currentConfig.spawnAngle)) * (spawnPoint - pivot) + pivot;
    }

    private void ConfigureWave(Wave wave, Color color, bool speedIsRandomizable)
    {
        float speed = currentConfig.regularSpeed;
        if (speedIsRandomizable && Random.Range(0.0f, 1.0f) >= 0.70f)
        {
            speed *= currentConfig.randomSpeedMultiplier;
        }
        wave.Configure(this, color, speed);
    }

    private void PositionWave(Wave wave, Transform ball, Vector3 spawnPosition, int offset, float firstWaveTimeToReach)
    {
        Vector3 direction = Vector3.Normalize(ball.position - spawnPosition);
        
        Transform waveTransform = wave.transform;
        float timeToReach = firstWaveTimeToReach + offset * currentConfig.waveDelay;
        waveTransform.position = -direction * (timeToReach * wave.moveSpeed + waveRadius + ballRadius) + ball.position;

        float angleInDegrees = Mathf.Rad2Deg * Mathf.Atan2(waveTransform.position.y - ball.position.y, waveTransform.position.x - ball.position.y);
        waveTransform.Rotate(new Vector3(0, 0, angleInDegrees + 180));

        wave.TakeOff(direction);
    }

    public void Reclaim(Wave wave)
    {
        waves.Remove(wave);
        waveFactory.Reclaim(wave);
    }

    public void StopRunning()
    {
        IsRunning = false;
        for (int i = 0; i <= waves.Count - 1; i++)
        {
            waves[i].DestroyWave();
        }
    }
}
