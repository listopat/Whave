using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

    public float moveSpeed { get; set; }

    private WaveController waveController;

    private Rigidbody2D rb;
    private Collider2D col;
    private Material material;

    private Transform originalTransform;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        material = GetComponent<Renderer>().material;
        col = GetComponent<Collider2D>();
    }

    public void Reset()
    {
        transform.rotation = Quaternion.identity;
        material.SetFloat("_Dissolve", 0.0f);
        col.enabled = true;
    }

    public void Configure(WaveController waveController, Color color, float speed)
    {   
        this.waveController = waveController;
        material.SetColor("_Tint", color);
        moveSpeed = speed;
    }
	
    public void SetController(WaveController waveController)
    {
        this.waveController = waveController;
    }

    public void TakeOff(Vector3 direction)
    {
        rb.velocity = direction * moveSpeed;
    }


    public void DestroyWave()
    {
        rb.velocity = Vector2.zero;
        col.enabled = false;
        StartCoroutine("DisintegrateWave");
    }

    IEnumerator DisintegrateWave()
    {
        for (float f = 0.045f; f <= 1.0; f += 0.045f)
        {
            material.SetFloat("_Dissolve", f);
            yield return null;
        }
        waveController.Reclaim(this);
    }
}
