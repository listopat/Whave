using UnityEngine;

[System.Serializable]
public class RandomSettings{
    public float minSpeed;
    public float maxSpeed;
}

[CreateAssetMenu]
public class StageConfig : ScriptableObject
{
    public Pattern[] patterns;
    public float patternDelay;
    public float waveDelay;
    public float regularSpeed;
    public int scoreToActivate;

    public bool randomizerEnabled;
    public RandomSettings randomSettings;
}
