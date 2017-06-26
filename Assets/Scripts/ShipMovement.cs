using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {

	private double speed;
	public int destSpeed { get; set; }       // Change setter to enforce max speed/rot speed
	public int maxSpeed { get; set; }
	public double accel { get; set; }
	public double rotSpeed { get; set; }
	public int destRotSpeed { get; set; }
	public int maxRotSpeed { get; set; }
	public double rotAccel { get; set; }
	private Vector2 direction;

	// Use this for initialization
	void Start () {
		speed = 0;
		destSpeed = 0;
		maxSpeed = 10;
		accel = 0.1;
		rotSpeed = 0;
		destRotSpeed = 0;
		maxRotSpeed = 5;
		rotAccel = 0.2;
		direction = Vector2.up;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(speed + accel < destSpeed) {
			speed += accel;
		} else if(speed - accel > destSpeed) {
			speed -= accel;
		} else {
			speed = destSpeed;
		}

		// DECIDE IF THIS SYSTEM WORKS FOR ROTATION - Use this or flat speed?
		if (rotSpeed + rotAccel < destRotSpeed) {
			rotSpeed += rotAccel;
		} else if (rotSpeed - rotAccel > destRotSpeed) {
			rotSpeed -= rotAccel;
		} else {
			rotSpeed = destRotSpeed;
		}

		transform.Translate(direction * (float)speed * Time.deltaTime);
		transform.Rotate(Vector3.forward * (float)rotSpeed);
	}
}
