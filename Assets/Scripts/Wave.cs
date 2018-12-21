using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

    public float moveSpeed { get; set; }
    public Vector3 direction { get; set;}

    private WaveFactory waveFactory;

	void Start () {
       GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;
	}
	
    public void SetFactory(WaveFactory waveFactory)
    {
        this.waveFactory = waveFactory;
    }

    public void DestroyWave()
    {
        waveFactory.DestroyWave(this);
    }
}
