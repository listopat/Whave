using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRing : MonoBehaviour {

    [SerializeField] private GameController game;
    [SerializeField] private bool isLeft;

    [SerializeField] private Hit hitEffect;
    ParticleSystem hitParticleSystem;

    private void Awake()
    {
        hitParticleSystem = hitEffect.GetComponent<ParticleSystem>();
    }

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
            Wave wave = other.GetComponent<Wave>();
            hitEffect.SetColorForParticles(wave.GetCurrentColor());
            hitParticleSystem.Play();
            wave.DestroyWave();
            game.WaveBlocked(isLeft);
        }
        else if (!isLeft && game.IsRightBallHitPressed)
        {
            Wave wave = other.GetComponent<Wave>();
            hitEffect.SetColorForParticles(wave.GetCurrentColor());
            hitParticleSystem.Play();
            wave.DestroyWave();
            game.WaveBlocked(isLeft);
        }
    }
}
