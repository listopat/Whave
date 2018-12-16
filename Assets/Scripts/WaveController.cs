﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {

    [SerializeField] private Wave wavePrefab;
    [SerializeField] private WaveFactory waveFactory;

    [SerializeField] private Transform leftBall;
    [SerializeField] private Transform rightBall;
    [SerializeField] private float ballXAxisOffset;

    private Vector3 leftSpawn = new Vector3(-22, -20, 0);
    private Vector3 rightSpawn = new Vector3(22, 20, 0);

    private StageConfig currentConfig;
    private float timeToNextPattern = 0;

    private readonly float waveRadius = 5f;
    private readonly float ballRadius = 0.5f;

    void Update()
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

    
    void CreatePattern()
    {
        
        Pattern pattern = currentConfig.patterns[Random.Range(0, currentConfig.patterns.Length)];
        Debug.Log(pattern);

        int offsetsSum = 0;
        float firstWaveTimeToReach = 0;

        for (int i = 0; i < pattern.waves.Length; i++)
        {
            Wave wave = waveFactory.getWave();
            ConfigureWave(wave);
            
            
            Transform ball = pattern.waves[i].side == Side.Left ? leftBall : rightBall;
            Vector3 spawnPosition = pattern.waves[i].side == Side.Left ? leftSpawn : rightSpawn;
            offsetsSum += pattern.waves[i].offset;

            if (i == 0)
            {
                firstWaveTimeToReach = ((ball.position - spawnPosition).magnitude - waveRadius - ballRadius) / wave.moveSpeed;
            }

            PositionWave(wave, ball, spawnPosition, offsetsSum, firstWaveTimeToReach);
        }
        
    }

    void ConfigureWave(Wave wave)
    {
        if (currentConfig.randomizerEnabled)
        {

        } else
        {
            wave.moveSpeed = currentConfig.regularSpeed;
        }
    }

    void PositionWave(Wave wave, Transform ball, Vector3 spawnPosition, int offset, float firstWaveTimeToReach)
    {
        Vector3 direction = Vector3.Normalize(ball.position - spawnPosition);
        wave.direction = direction;

        Transform waveTransform = wave.transform;
        float timeToReach = firstWaveTimeToReach + offset * currentConfig.waveDelay;
        waveTransform.position = -direction * (timeToReach * wave.moveSpeed + waveRadius + ballRadius) + ball.position;

        float angleInDegrees = Mathf.Rad2Deg * Mathf.Atan2(waveTransform.position.y - ball.position.y, waveTransform.position.x - ball.position.y);
        waveTransform.Rotate(new Vector3(0, 0, angleInDegrees + 180));
    }

}
