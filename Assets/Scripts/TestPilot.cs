using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPilot : MonoBehaviour {

	ShipMovement moveScript;

	// Use this for initialization
	void Start () {
		moveScript = GameObject.FindWithTag("Ship").GetComponent<ShipMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("w")) {
			moveScript.destSpeed = 5;
		} else if(Input.GetKeyUp("w")) {
			moveScript.destSpeed = 0;
		}
		if (Input.GetKeyDown("a")) {
			moveScript.destRotSpeed = 120;
		} else if (Input.GetKeyUp("a")) {
			moveScript.destRotSpeed = 0;
		}
		if (Input.GetKeyDown("s")) {
			moveScript.destSpeed = -5;
		} else if (Input.GetKeyUp("s")) {
			moveScript.destSpeed = 0;
		}
		if (Input.GetKeyDown("d")) {
			moveScript.destRotSpeed = -120;
		} else if (Input.GetKeyUp("d")) {
			moveScript.destRotSpeed = 0;
		}
	}


}
