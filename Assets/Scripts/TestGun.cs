using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGun : MonoBehaviour {

	GunBarrel gunScript;

	// Use this for initialization
	void Start () {
		gunScript = GameObject.FindWithTag("Gun").GetComponent<GunBarrel>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("q")) {
			gunScript.destRot += 20 * Time.deltaTime;
		}
		if (Input.GetKey("e")) {
			gunScript.destRot -= 20 * Time.deltaTime;
		}
	}
}
