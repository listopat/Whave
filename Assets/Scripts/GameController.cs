using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField] private Ball leftBall;
    [SerializeField] private Ball rightBall;

    [SerializeField] private WaveController waveController;
    [SerializeField] private StageConfig[] stageConfigs;

    private int score = 0;

    void Start () {
        waveController.SetConfig(stageConfigs[0]);
    }
	
	void Update () {

    }

    void WaveBlocked()
    {
        score += 1;
    }
}
