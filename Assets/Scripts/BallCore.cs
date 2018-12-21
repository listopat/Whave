using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCore : MonoBehaviour {

    [SerializeField] private GameController game;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Wave>().DestroyWave();
        game.StopGame();
    }
}
