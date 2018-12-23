using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCore : MonoBehaviour {

    [SerializeField] private GameController game;

    private Material mat;

    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Wave>().DestroyWave();
        mat.SetFloat("_EnableCrack", 1);
        game.StopGame();
    }

    public void ResetCrack()
    {
        mat.SetFloat("_EnableCrack", 5000);
    }
}
