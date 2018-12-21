﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRing : MonoBehaviour {

    [SerializeField] private GameController game;
    [SerializeField] private bool isLeft;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckInput(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckInput(other);
    }

    private void CheckInput(Collider2D other)
    {
        if (isLeft && game.IsLeftBallHitPressed)
        {
            other.GetComponent<Wave>().DestroyWave();
            game.WaveBlocked();
        }
        else if (!isLeft && game.IsRightBallHitPressed)
        {
            other.GetComponent<Wave>().DestroyWave();
            game.WaveBlocked();
        }
    }
}
