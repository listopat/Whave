using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour {

    ParticleSystem.MainModule particleMainModule;

    private void Awake()
    {
        particleMainModule = GetComponent<ParticleSystem>().main;
    }

    public void SetColorForParticles(Color color)
    {
        particleMainModule.startColor = color;
    }
}
