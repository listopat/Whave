using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WaveFactory : ScriptableObject {

    [SerializeField] private Wave wavePrefab;

    List<Wave> waves;

    void Awake ()
    {
        waves = new List<Wave>();
    }

    public Wave getWave()
    {
        Wave wave = Instantiate(wavePrefab);
        waves.Add(wave);
        return wave;
    }

    public void DestroyWave(Wave wave)
    {

    }
}
