using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

    public float moveSpeed { get; set; }
    public Vector3 direction { get; set;}

    private WaveFactory waveFactory;

    private Rigidbody2D rb;
    private Material material;

	void Start () {
       rb = GetComponent<Rigidbody2D>();
       rb.velocity = direction * moveSpeed;

       material = GetComponent<Renderer>().material;
    }
	
    public void SetFactory(WaveFactory waveFactory)
    {
        this.waveFactory = waveFactory;
    }

    public void DestroyWave()
    {
        rb.velocity = Vector2.zero;
        StartCoroutine("DisintegrateWave");
    }

    IEnumerator DisintegrateWave()
    {
        for (float f = 0.05f; f <= 1.0; f += 0.05f)
        {
            material.SetFloat("_Dissolve", f);
            yield return null;
        }
        waveFactory.DestroyWave(this);
    }
}
