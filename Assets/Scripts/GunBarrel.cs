using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBarrel : MonoBehaviour {

	// Note: If max motor force set too high, hinge will get stuck at limits - keep around 50

	private float _rotSpeed;
	public float rotSpeed {
		get { return _rotSpeed; }
		set {
			_rotSpeed = value;
			//JointMotor2D motor = hinge.motor;
			//motor.motorSpeed = _rotSpeed;
			//hinge.motor = motor;
		}
	}

	private bool turning;
	private bool turnDir;

	private HingeJoint2D hinge;

	// Use this for initialization
	void Start () {
		rotSpeed = 30;
		//setLimits(-75, 75);
		hinge = GetComponent<HingeJoint2D>();
		hinge.useMotor = true;
	}

	public void initialize(float rotSpeed, float minRot, float maxRot) {
		this.rotSpeed = rotSpeed;
		setLimits(minRot, maxRot);
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {

	}

	// Set min and max angle hinge can turn to
	public void setLimits(float min, float max) {
		JointAngleLimits2D limits = hinge.limits;
		limits.max = max;
		limits.min = min;
		hinge.limits = limits;
	}

	// Begin turning barrel - true for left, false for right
	public void turn(bool dir) {
		float turnSpd = dir ? -rotSpeed : rotSpeed;
		JointMotor2D motor = hinge.motor;
		motor.motorSpeed = turnSpd;
		hinge.motor = motor;
	}

	public void stopTurn() {
		JointMotor2D motor = hinge.motor;
		motor.motorSpeed = 0;
		hinge.motor = motor;
	}

	void Fire() {
		//GameObject bullet = Instantiate();
	}
}
