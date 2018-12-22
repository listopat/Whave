using System.Collections.Generic;
using UnityEngine;

public class WaveFactory : MonoBehaviour {

    [SerializeField] private Wave wavePrefab;

    private List<Wave> pool;

    void Awake ()
    {
        pool = new List<Wave>();
    }

    public Wave getWave()
    {
        Wave wave;

        int lastIndex = pool.Count - 1;
        if (lastIndex >= 0)
        {
            wave = pool[lastIndex];
            pool.RemoveAt(lastIndex);
            wave.gameObject.SetActive(true);
            wave.Reset();
        } else
        {
            wave = Instantiate(wavePrefab);
        }
        return wave;
    }

    public void Reclaim(Wave wave)
    {
        pool.Add(wave);
        wave.gameObject.SetActive(false);
    }
}
