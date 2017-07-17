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
		if (Input.GetKeyDown("q")) {
			gunScript.turn(true);
		} else if (Input.GetKeyUp("q")) {
			gunScript.stopTurn();
		}
		if (Input.GetKeyDown("e")) {
			gunScript.turn(false);
		} else if (Input.GetKeyUp("e")) {
			gunScript.stopTurn();
		}
	}
}
