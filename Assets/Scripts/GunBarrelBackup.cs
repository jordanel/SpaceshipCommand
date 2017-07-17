using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBarrelBackup : MonoBehaviour {

	float rot;
	public float destRot { get; set; }
	float rotSpeed;
	float maxRot;

	// Use this for initialization
	void Start() {
		rot = 0;
		destRot = 0;
		rotSpeed = 20;
		maxRot = 180;

		Debug.Log(transform.parent.position);
	}

	float rotSpeedT = 0;
	// Update is called once per frame
	void Update() {
		rotSpeedT = rotSpeed * Time.deltaTime;

		if (rot + rotSpeedT < destRot) {
			rot += rotSpeedT;
			transform.RotateAround(transform.parent.position, Vector3.forward, rotSpeedT);
		} else if (rot - rotSpeedT > destRot) {
			rot -= rotSpeedT;
			transform.RotateAround(transform.parent.position, Vector3.forward, -rotSpeedT);
		} else {
			//rot = destRot;
		}

	}

	void Fire() {
		//GameObject bullet = Instantiate();
	}
}
