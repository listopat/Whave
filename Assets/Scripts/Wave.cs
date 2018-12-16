using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

    public float moveSpeed { get; set; }
    public Vector3 direction { get; set;}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += direction * moveSpeed * Time.deltaTime;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("trigger wave");
    }

}
