using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

    [SerializeField] private float moveSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger wave");
    }
}
