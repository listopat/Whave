using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour {

    ParticleSystem ps;
    ParticleSystem.Particle[] particleBuffer;

    Color currentColor = new Color(0.15f, 0.15f, 0.15f);

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        particleBuffer = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    private void LateUpdate()
    {
        int numParticlesAlive = ps.GetParticles(particleBuffer);

        if (numParticlesAlive > 0)
        {
            for (int i = 0; i < numParticlesAlive; i++)
            {
                particleBuffer[i].startColor = currentColor;
            }
            ps.SetParticles(particleBuffer, numParticlesAlive);
        }
    }

    public void SetColorForParticles(Color color)
    {
        currentColor = color;
    }
}
