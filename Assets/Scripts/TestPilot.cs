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
			moveScript.changeThrust(2, Vector2.up);
			//moveScript.destSpeed = 5;
		} else if(Input.GetKeyUp("w")) {
			//moveScript.destSpeed = 0;
			moveScript.thrusting = false;
		}
		if (Input.GetKeyDown("a")) {
			moveScript.changeTurnSpd(90);
		} else if (Input.GetKeyUp("a")) {
			moveScript.changeTurnSpd(0);
		}
		if (Input.GetKeyDown("s")) {
			moveScript.changeThrust(2, Vector2.down);
			//moveScript.destSpeed = -5;
		} else if (Input.GetKeyUp("s")) {
			//moveScript.destSpeed = 0;
			moveScript.thrusting = false;
		}
		if (Input.GetKeyDown("d")) {
			moveScript.changeTurnSpd(-90);
		} else if (Input.GetKeyUp("d")) {
			moveScript.changeTurnSpd(0);
		}
	}


}
