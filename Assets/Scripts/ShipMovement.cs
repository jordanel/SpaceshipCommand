using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {

	// TODO: Add initialization function to set maxSpeed, accel, etc.

	public float maxSpeed { get; set; }
	/*private float _destSpeed;
	public float destSpeed {
		get { return _destSpeed; }
		set { if(value < maxSpeed) { _destSpeed = value; } else { _destSpeed = maxSpeed; } }
	}*/
	public float thrust { get; set; }

	public float maxRotSpeed { get; set; }    // Consider revMaxSpeed or making maxSpeed limit both dirs
	private float _destRotSpeed;
	public float destRotSpeed {
		get { return _destRotSpeed; }
		set { if (value < maxRotSpeed) { _destRotSpeed = value; } else { _destRotSpeed = maxRotSpeed; } }
	}

	public bool thrusting { get; set; }  // apply thrust or not
	public Vector2 direction { get; set; }

	public bool rotating { get; set; }  // rotating or not
	public float torky { get; set; }
	public bool turnDir { get; set; } // left or right

	Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
		maxSpeed = 10;
		thrust = 2;
		destRotSpeed = 0;
		maxRotSpeed = 180;
		torky = 1;

		direction = Vector2.up;

		rigidBody = GetComponent<Rigidbody2D>();
	}

	public void initialize(float maxSpeed, float thrust, float maxRotSpeed, float torky) {
		this.maxSpeed = maxSpeed;
		this.thrust = thrust;
		this.maxRotSpeed = maxRotSpeed;
		this.torky = torky;
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		// Apply thrust to ship - amount of force controlled directly by player
		if(thrusting) {
			if(rigidBody.velocity.magnitude >= maxSpeed) {      // Stop thrusting upon reaching max speed
				thrusting = false;
				thrust = 0;
			} else {
				rigidBody.AddRelativeForce(thrust * direction);
			}
		}

		// Apply torque to ship - desired angular velocity controlled by player
		if(rotating) {
			if((rigidBody.angularVelocity <= destRotSpeed && !turnDir) || (rigidBody.angularVelocity >= destRotSpeed && turnDir)) {     // Desired rot speed reached
				rotating = false;
				rigidBody.angularVelocity = destRotSpeed;
			} else {
				if(turnDir) {
					rigidBody.AddTorque(torky);    // Turning left
				} else {
					rigidBody.AddTorque(-torky);   // Turning right
				}
			}
		}
	}

	// Adjust thrust magnitude and direction
	public void changeThrust(float thrust, Vector2 direction) {
		this.thrust = thrust;
		this.direction = direction.normalized;
		thrusting = true;
	}

	// Change desired rotation speed
	public void changeTurnSpd(float angVel) {
		destRotSpeed = angVel;
		turnDir = rigidBody.angularVelocity < angVel;
		rotating = true;
	}
}
