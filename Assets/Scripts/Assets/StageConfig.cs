using UnityEngine;

[CreateAssetMenu]
public class StageConfig : ScriptableObject
{
    public Pattern[] patterns;
    public float patternDelay;
    public float waveDelay;
    public float regularSpeed;
    public int scoreToActivate;

    public float randomSpeedMultiplier;
    public float spawnAngle;
}
