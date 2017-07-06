using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {

	// TODO: Add initialization function to set maxSpeed, accel, etc.

	private float speed;
	public float maxSpeed { get; set; }
	private float _destSpeed;
	public float destSpeed {
		get { return _destSpeed; }
		set { if(value < maxSpeed) { _destSpeed = value; } else { _destSpeed = maxSpeed; } }
	}
	public float accel { get; set; }

	public float rotSpeed { get; set; }
	public float maxRotSpeed { get; set; }    // Consider revMaxSpeed or making maxSpeed limit both dirs
	private float _destRotSpeed;
	public float destRotSpeed {
		get { return _destRotSpeed; }
		set { if (value < maxRotSpeed) { _destRotSpeed = value; } else { _destRotSpeed = maxRotSpeed; } }
	}
	public float rotAccel { get; set; }

	private Vector2 direction;

	Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
		speed = 0;
		destSpeed = 0;
		maxSpeed = 10;
		accel = 2;
		rotSpeed = 0;
		destRotSpeed = 0;
		maxRotSpeed = 180;
		rotAccel = 60;
		direction = Vector2.up;

		rigidBody = GetComponent<Rigidbody2D>();
	}


	float accelT = 0;
	float rotAccelT = 0;
	// Update is called once per frame
	void Update () {

		accelT = accel * Time.deltaTime;
		rotAccelT = rotAccel * Time.deltaTime;

		if(speed + accelT < destSpeed) {
			speed += accelT;
		} else if(speed - accelT > destSpeed) {
			speed -= accelT;
		} else {
			speed = destSpeed;
		}

		// DECIDE IF THIS SYSTEM WORKS FOR ROTATION - Use this or flat speed?
		if (rotSpeed + rotAccelT < destRotSpeed) {
			rotSpeed += rotAccelT;
		} else if (rotSpeed - rotAccelT > destRotSpeed) {
			rotSpeed -= rotAccelT;
		} else {
			rotSpeed = destRotSpeed;
		}

		transform.Translate(direction * speed * Time.deltaTime);
		transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
	}

	void FixedUpdate() {
		if(rigidBody.velocity.magnitude > Mathf.Abs(destSpeed)) {
			
		}
	}
}
